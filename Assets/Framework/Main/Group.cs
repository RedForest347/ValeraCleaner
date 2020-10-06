using System;
using System.Collections;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Linq;
using UnityEngine;
using System.Reflection;

namespace RangerV
{
    /// <summary>
    /// создание группы:
    /// Group group = Group.Create(new ComponentsList<SomeComp1, SomeComp2>() -- компоненты --, new ComponentsList<SomeComp3, SomeComp4> -- исключения --);
    /// применяемые интерфейсы:
    /// ICustomAwake, ICustomStart, ICustomUpdate (ICustomFixedUpdate, ICustomLateUpdate), IStoppable, ICustomDisable, IReceive<T>
    /// </summary>
    /// <idea>
    /// Идея по оптимизации GetEnumerator
    /// GetEnumerator будет записываться в переменную и обновляться только при добавлении или удалении сущности, а не при заждом обращении к GetEnumerator
    /// </idea>
    public class Group
    {

        #region Static Func

        static List<Group> groups = new List<Group>();

        public static Group Create(ComponentsList Components)
        {
            return Create(Components, ComponentsList.Empty);
        }


        /// <summary>
        /// подразумевается, что Components и Exceptions будут созданы с помощью new ComponentsList<>()
        /// в противном случае, могут возникнуть ошибки
        /// </summary>
        /// <param name="Components">компоненты, которые обязаны быть в группе</param>
        /// <param name="Exceptions">компоненты, которых обязано не быть в группе</param>
        /// <returns>возвращает ссылку на созданную/существующую группу</returns>
        public static Group Create(ComponentsList Components, ComponentsList Exceptions)
        {
            if (Components == null)
                Debug.LogException(new Exception("при создании группы лист компонентов пуст"));

            if (Exceptions == null)
                Debug.LogException(new Exception("при создании группы лист исключений пуст (используй Group.Create(ComponentsList Components))"));

            if (Components.types.GroupBy(g => g).Where(w => w.Count() > 1).Count() != 0)
                Debug.LogException(new Exception("повторяющиеся компоненты при создании группы"));

            if (Exceptions != null && Exceptions.types.GroupBy(g => g).Where(w => w.Count() > 1).Count() != 0)
                Debug.LogException(new Exception("повторяющиеся исключения при создании группы"));



            Group group = new Group(Components.types, Exceptions.types);

            Group exist_group = GetExistGroup(group);

            if (exist_group != null)
            {
                exist_group.group_per_instance++;
                return exist_group;
            }

            group.InitDictionary();
            group.InitGroupEvents();

            groups.Add(group);

            return group;
        }

        public static void Clear()
        {
            for (int i = 0; i < groups.Count; i++)
                groups[i].DeleteBase();

            groups = new List<Group>();
        }

        static Group GetExistGroup(Group group)
        {
            for (int i = 0; i < groups.Count; i++)
                if (groups[i] == group)
                    return groups[i];

            return null;
        }

        public static bool operator ==(Group group1, Group group2)
        {
            if (group1 is null || group2 is null)
                return ReferenceEquals(group1, group2);

            return group1.hash_code_components == group2.hash_code_components && group1.hash_code_exceptions == group2.hash_code_exceptions;
        }

        public static bool operator !=(Group group1, Group group2)
        {
            return !(group1 == group2);
        }


        #endregion Static Func



        

        public int entities_count { get; private set; }

        Dictionary<int, EntContainer> EntitiesDictionary;

        List<Type> Components;
        List<Type> Exceptions;

        int group_per_instance; // сколько групп ссылается на данную ссылку

        long hash_code_components;
        long hash_code_exceptions;

        public event Action<int> OnAddEntity;
        public event Action<int> OnRemoveEntity;

        private Group(List<Type> Components) : this(Components, new List<Type>(0)) { }

        private Group(List<Type> Components, List<Type> Exceptions)
        {
            if (Components == null)
                throw new Exception("при создании группы, лист Components пуст");
            if (Exceptions == null)
                throw new Exception("при создании группы, лист Exceptions пуст");

            entities_count = 0;
            group_per_instance = 1;
            this.Components = Components;
            this.Exceptions = Exceptions;

            for (int i = 0; i < Components.Count; i++)
                hash_code_components += Components[i].GetHashCode();
            for (int i = 0; i < Exceptions.Count; i++)
                hash_code_exceptions += Exceptions[i].GetHashCode();

        }

        void AddEntity(int entity)
        {
            EntitiesDictionary[entity].was_added = true;
            entities_count++;
            OnAddEntity?.Invoke(entity);
        }


        void CheckEntityOnRemove(int ent)
        {
            if (EntitiesDictionary[ent].was_added)
            {
                OnRemoveEntity?.Invoke(ent);
                RemoveEntity(ent);
            }
        }

        void RemoveEntity(int entity)
        {
            EntitiesDictionary[entity].was_added = false;
            entities_count--;

            if (EntityBase.GetEntity(entity) == null)
                throw new Exception("Сущность == null при ее удалении)");
        }

        public List<Type> GetComponentTypes()
        {
            List<Type> componentsTypes = new List<Type>(Components.Count);
            for (int i = 0; i < Components.Count; i++)
                componentsTypes.Add(Components[i]);

            return componentsTypes;
        }

        public List<Type> GetExceptionTypes()
        {
            List<Type> exceptionsTypes = new List<Type>(Exceptions.Count);
            for (int i = 0; i < Exceptions.Count; i++)
                exceptionsTypes.Add(Exceptions[i]);

            return exceptionsTypes;
        }

        void InitDictionary()
        {
            int length = EntityBase.entity_count;

            EntitiesDictionary = new Dictionary<int, EntContainer>();

            for (int ent = 0; ent < length; ent++)
            {
                EntitiesDictionary.Add(ent, EntContainer.Empty);

                if (EntityBase.GetEntity(ent) != null)
                {
                    EntContainer entContainer = new EntContainer(ent, Components.Count);

                    for (int comp = 0; comp < Components.Count; comp++)
                        if (Storage.ContainsComponent(Components[comp], ent))
                            entContainer.remains_components--;

                    for (int exc = 0; exc < Exceptions.Count; exc++)
                        if (Storage.ContainsComponent(Exceptions[exc], ent))
                            entContainer.remains_exceptions++;

                    EntitiesDictionary[ent] = entContainer;

                    if (entContainer.ShouldJoin())
                        AddEntity(ent);
                }
            }
        }

        void InitGroupEvents()
        {
            EntityBase.OnBeforeAddComponents += OnCreateNewEntityID;


            for (int comp = 0; comp < Components.Count; comp++)
            {
                Storage.GetStorage(Components[comp]).OnAdd += OnAddComponent;
                Storage.GetStorage(Components[comp]).OnBeforeRemove += OnRemoveComponent;
            }

            for (int exceptions = 0; exceptions < Exceptions.Count; exceptions++)
            {
                Storage.GetStorage(Exceptions[exceptions]).OnAdd += OnAddException;
                Storage.GetStorage(Exceptions[exceptions]).OnBeforeRemove += OnRemoveException;
            }
        }

        void FinalEvents()
        {
            EntityBase.OnBeforeAddComponents -= OnCreateNewEntityID;

            if (Components != null)
            {
                for (int comp = 0; comp < Components.Count; comp++)
                {
                    Storage.GetStorage(Components[comp]).OnAdd -= OnAddComponent;
                    Storage.GetStorage(Components[comp]).OnBeforeRemove -= OnRemoveComponent;
                }
            }


            if (Exceptions != null)
            {
                for (int exceptions = 0; exceptions < Exceptions.Count; exceptions++)
                {
                    Storage.GetStorage(Exceptions[exceptions]).OnAdd -= OnAddException;
                    Storage.GetStorage(Exceptions[exceptions]).OnBeforeRemove -= OnRemoveException;
                }
            }
        }

        void OnCreateNewEntityID(int entity)
        {
            if (!EntitiesDictionary.ContainsKey(entity))
            {
                EntitiesDictionary.Add(entity, null);
            }

            EntContainer entContainer = new EntContainer(entity, Components.Count);

            for (int comp = 0; comp < Components.Count; comp++)
                if (Storage.ContainsComponent(Components[comp], entity))
                {
                    Debug.LogWarning("При добавлении сущности в группу, на ней не должны находится компоненты, " +
                        "по идее. сейчас находятся. работа продолжится в штатном режиме");
                    entContainer.remains_components--;
                }

            for (int exc = 0; exc < Exceptions.Count; exc++)
                if (Storage.ContainsComponent(Exceptions[exc], entity))
                {
                    Debug.LogWarning("При добавлении сущности в группу, на ней не должны находится компоненты, " +
                        "по идее. сейчас находятся. работа продолжится в штатном режиме");
                    entContainer.remains_exceptions++;
                }

            EntitiesDictionary[entity] = entContainer;

            if (EntitiesDictionary[entity].ShouldJoin())
                AddEntity(entity);
        }

        void OnAddComponent(int entity)
        {
            if (!EntitiesDictionary.ContainsKey(entity))
                Debug.LogError("в библиотеке " + GetHashCode() + " не найдена сущность " + entity);

            EntitiesDictionary[entity].remains_components--;

            if (EntitiesDictionary[entity].ShouldJoin())
                AddEntity(entity);
        }

        void OnRemoveComponent(int ent)
        {
            EntitiesDictionary[ent].remains_components++;

            CheckEntityOnRemove(ent);
        }

        void OnAddException(int ent)
        {
            EntitiesDictionary[ent].remains_exceptions++;

            CheckEntityOnRemove(ent);
        }

        void OnRemoveException(int ent)
        {
            EntitiesDictionary[ent].remains_exceptions--;
            if (EntitiesDictionary[ent].ShouldJoin())
                AddEntity(ent);
        }

        ///пользоваться осторожно
        public void Delete() // функция, которая обнуляет группу
        {
            if (group_per_instance != 1)
                Debug.LogWarning("На данную группу ссылаются и из других мест. Ее удаление в данном случае не рекомендуется (удаление произойдет)");
            DeleteBase();
        }

        void DeleteBase()
        {
            FinalEvents();
            Components = Exceptions = null;
            hash_code_components = hash_code_exceptions = 0;
            groups.Remove(this);
        }

        public int[] GetEntitiesArray()
        {
            int[] Entities = new int[entities_count];
            int next = 0;

            EntContainer[] array = EntitiesDictionary.Values.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i].was_added)
                    Entities[next++] = array[i].entity;

            return Entities;
        }


        public bool Contains(int entity)
        {
            EntitiesDictionary.TryGetValue(entity, out EntContainer entContainer);

            return entContainer?.was_added ?? false;
        }

        #region Equals/HashCode/Enumerator


        public override bool Equals(object obj)
        {
            return obj.GetType() != GetType() ? false : this == (Group)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IEnumerator<int> GetEnumerator()
        {
            EntContainer[] array = EntitiesDictionary.Values.ToArray();

            for (int i = 0; i < array.Length; i++)
                if (array[i].was_added)
                    yield return array[i].entity;
        }

        #endregion Equals/HashCode/Enumerator

        #region Test

        public void InitEvents(Action<int> OnAdd, Action<int> OnRemove)
        {
            foreach (int button in this)
                OnAdd(button);

            OnAddEntity += OnAdd;
            OnRemoveEntity += OnRemove;
        }

        public void DeinitEvents(Action<int> OnAdd, Action<int> OnRemove)
        {
            foreach (int button in this)
                OnRemove(button);

            OnAddEntity -= OnAdd;
            OnRemoveEntity -= OnRemove;
        }


        #endregion


        class EntContainer
        {
            public int remains_components; // сколько компонентов осталось до полного набора
            public int remains_exceptions; // сколько осталось лишних исключений
            public int entity;
            public bool was_added; // была ли сущность добавлена в группу
            int start_num_of_remains_components;

            public static EntContainer Empty { get => new EntContainer(); }

            EntContainer() : this(-2, -2) { }

            public EntContainer(int entity, int num_of_components)
            {
                remains_components = num_of_components;
                remains_exceptions = 0;
                this.entity = entity;
                was_added = false;
                start_num_of_remains_components = num_of_components;
            }

            public bool ShouldJoin()
            {
                if (remains_components < 0)
                    Debug.LogError("remains_components меньше нуля " + remains_components);
                if (remains_exceptions < 0)
                    Debug.LogError("remains_exceptions меньше нуля " + remains_exceptions);
                if (entity <= 0)
                    Debug.LogError("entity <= 0");



                /*if (remains_components == 0 && remains_exceptions == 0 && entity >= 0)
                    Debug.Log("сущность " + entity + " следует добавить в группу " + start_num_of_remains_components);
                else
                    Debug.Log("сущность " + entity + " следует добавить в группу " + start_num_of_remains_components);*/

                return remains_components == 0 && remains_exceptions == 0 && entity >= 0;
            }

            public void Zeroing()
            {
                remains_components = start_num_of_remains_components;
                remains_exceptions = 0;
                entity = -1;
                was_added = false;
            }

            public static explicit operator int(EntContainer container)
            {
                return container.entity;
            }
        }
    }

    #region ComponentsList

    public class ComponentsList
    {
        public List<Type> types;
        public static ComponentsList Empty { get => new ComponentsList(); }

        public ComponentsList()
        {
            types = new List<Type>();
        }
    }

    public class ComponentsList<T1> : ComponentsList
                                        where T1 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            types = new List<Type>
            {
                typeof(T1)
            };
        }
    }

    public class ComponentsList<T1, T2> : ComponentsList
                                        where T1 : ComponentBase, IComponent, new()
                                        where T2 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2)
            };
        }
    }

    public class ComponentsList<T1, T2, T3> : ComponentsList
                                       where T1 : ComponentBase, IComponent, new()
                                       where T2 : ComponentBase, IComponent, new()
                                       where T3 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3)
            };
        }
    }

    public class ComponentsList<T1, T2, T3, T4> : ComponentsList
                                       where T1 : ComponentBase, IComponent, new()
                                       where T2 : ComponentBase, IComponent, new()
                                       where T3 : ComponentBase, IComponent, new()
                                       where T4 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4)
            };
        }
    }

    public class ComponentsList<T1, T2, T3, T4, T5> : ComponentsList
                                      where T1 : ComponentBase, IComponent, new()
                                      where T2 : ComponentBase, IComponent, new()
                                      where T3 : ComponentBase, IComponent, new()
                                      where T4 : ComponentBase, IComponent, new()
                                      where T5 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5)
            };
        }
    }

    public class ComponentsList<T1, T2, T3, T4, T5, T6> : ComponentsList
                                      where T1 : ComponentBase, IComponent, new()
                                      where T2 : ComponentBase, IComponent, new()
                                      where T3 : ComponentBase, IComponent, new()
                                      where T4 : ComponentBase, IComponent, new()
                                      where T5 : ComponentBase, IComponent, new()
                                      where T6 : ComponentBase, IComponent, new()
    {
        public ComponentsList()
        {
            types = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5),
                typeof(T6)
            };
        }
    }

    #endregion ComponentsList
}
