using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using RangerV.Support;

namespace RangerV
{
    public abstract class EntityBase : MonoBehaviour
    {
        public static event Action<int> OnCreateEntityID;
        public static event Action<int> OnBeforeAddComponents;
        public static event Action<int> OnDestroyEntity;
        public static event Action<int> OnActivateEntity;


        static EntityBase[] Entities { get => EntityBaseData.Instance.Entities; }
        static Stack<int> freeID { get => EntityBaseData.Instance.freeID; }
        static int nextMax { get => EntityBaseData.Instance.nextMax; set => EntityBaseData.Instance.nextMax = value; } 

        public static int entity_count { get => EntityBaseData.Instance.nextMax; }

        public int entity;

        [SerializeField]
        public List<ComponentBase> Components = new List<ComponentBase>();
        public EntityState state;

        public static EntityBase GetEntity(int entity)
        {
            return Entities[entity];
        }

        public static bool ContainsEntity(int entity)
        {
            return Entities[entity] != null;
        }

        public void Awake()
        {
            state.runtime = true;
            CreateEntityID();

            if (!Starter.initialized)
                state.requireStarter = true;
            else
                SetupAfterStarter();
        }

        private void OnEnable()
        {
            if (state.requireStarter)
                return;
            if (state.enabled)
                return;

            state.enabled = true;
            Entities[entity] = this;
            OnBeforeAddComponents?.Invoke(entity);

            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is ICustomAwake)
                    ((ICustomAwake)Components[i]).OnAwake();

                ManagerUpdate.Add(Components[i]);
                Storage.AddComponent(Components[i], entity);
            }

            OnActivateEntity?.Invoke(entity);
        }

        private void OnDisable()
        {
            OnDeactivate();
        }

        private void OnDestroy()
        {
            state.runtime = false;
            freeID.Push(entity);
            entity = -1;
        }

        public void OnDeactivate()
        {
            
            state.enabled = false;
            ManagerUpdate.Remove(this);
            Storage.RemoveFromAllStorages(entity);

            Entities[entity] = null;

            OnDestroyEntity?.Invoke(entity);
        }


        #region MAIN


        public void SetupAfterStarter()
        {
            state.requireStarter = false;
            OnEnable();
            Setup();
            state.initialized = true;
        }

        void CreateEntityID()
        {
            if (Entities.Length <= nextMax)
                Array.Resize(ref EntityBaseData.Instance.Entities, Entities.Length + 10);

            if (freeID.Count > 0)
                entity = freeID.Pop();
            else
                entity = nextMax++;

            Entities[entity] = this;
            OnCreateEntityID?.Invoke(entity);
        }

        #endregion MAIN

        #region ADD/REMOVE

        public T AddCmp<T>() where T : ComponentBase, IComponent, new()
        {
            return (T)AddCmp(typeof(T));
        }

        public ComponentBase AddCmp(Type componentType)
        {
            if (componentType == typeof(ComponentBase))
                throw new Exception("Попытка добавить ComponentBase " + componentType);

            if (!state.runtime)
                return AddCmpInEditorMode(componentType);

            if (Storage.ContainsComponent(componentType, entity))
            {
                Debug.LogWarning("попытка добавить уже существующий компонент " + componentType + " к сущности " + entity + 
                    " (" + GetEntity(entity).gameObject.name + ")." + " компонент добавлен не будет");
                return null;
            }

            ComponentBase component = (ComponentBase)gameObject.AddComponent(componentType);

            if (component is ICustomAwake)
                ((ICustomAwake)component).OnAwake();

            ManagerUpdate.Add(component);

            Components.Add(component);
            (this as Entity).show_comp.Add(false);

            Storage.AddComponent(component, entity);
            
            return component;
        }


        ComponentBase AddCmpInEditorMode(Type componentType)
        {
            if (Components.Any(comp => comp.GetType() == componentType))
            {
                Debug.LogError("попытка добывления повторяющегося компонента " + componentType + " на сущность. Компонент добавлен не будет");
                return null;
            }

            ComponentBase _component = (ComponentBase)gameObject.AddComponent(componentType);
            Components.Add(_component);
            (this as Entity).show_comp.Add(false);

            return _component;
        }

        public bool RemoveCmp<T>() where T : ComponentBase, IComponent, new()
        {
            return RemoveCmp(typeof(T));
            /*if (!state.runtime)
                return RemoveCmpInEditorMode(typeof(T));

            if (!Storage.ContainsComponent<T>(entity))
                return false;

            Destroy(GetCmp<T>());
            Storage.RemoveComponent<T>(entity);
            RemoveCmpFromLists(typeof(T));
            return true;*/
        }

        public bool RemoveCmp(Type componentType)
        {
            if (!state.runtime)
                return RemoveCmpInEditorMode(componentType);

            if (!Storage.ContainsComponent(componentType, entity))
                return false;

            Destroy(GetCmp(componentType));
            Storage.RemoveComponent(componentType, entity);
            RemoveCmpFromLists(componentType);
            return true;
        }

        bool RemoveCmpFromLists(Type componentType)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].GetType() == componentType)
                {
                    Components.RemoveAt(i);
                    (this as Entity).show_comp.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        bool RemoveCmpInEditorMode(Type componentType)
        {
            int index = IndexOf(componentType);

            if (index == -1)
                return false;

            DestroyImmediate(Components[index]);
            Components.RemoveAt(index);
            (this as Entity).show_comp.RemoveAt(index);
            return true;
        }

        int IndexOf(Type ComponentType)
        {
            for (int i = 0; i < Components.Count; i++)
                if (Components[i].GetType() == ComponentType)
                    return i;

            return -1;
        }

        #endregion ADD/REMOVE

        #region GetComponent

        public List<ComponentBase> GetAllComponents()
        {
            List<ComponentBase>  componentBases = new List<ComponentBase>(Components.Count);

            for (int i = 0; i < Components.Count; i++)
                componentBases.Add(Components[i]);

            return componentBases;
        }

        public T GetCmp<T>() where T : ComponentBase, IComponent, new()
        {
            return Storage.GetComponent<T>(entity);
        }

        public ComponentBase GetCmp(Type type)
        {
            if (type == null)
                Debug.LogException(new ArgumentNullException("parametr name: type"));

            return Storage.GetComponent(type, entity);
        }

        public bool ContainsCmp<T>() where T : ComponentBase, IComponent, new()
        {
            return Storage.ContainsComponent<T>(entity) ? true : false;
        }

        public bool ContainsCmp(Type type)
        {
            if (type == null)
                Debug.LogException(new ArgumentNullException("parametr name: type"));

            return Storage.ContainsComponent(type, entity) ? true : false;
        }

        #endregion GetComponent

        public virtual void Setup() { }




    }
}


namespace RangerV.Support
{
    /// <summary>
    /// костыль. его суть - хранить статические данные для EntityBase на синглтоне
    /// </summary>
    public class EntityBaseData : MonoBehaviour
    {
        public static EntityBaseData Instance { get => Singleton<EntityBaseData>.Instance; }

        /// <summary>
        /// нулевой элемент не должен быть занят
        /// </summary>
        public EntityBase[] Entities;
        public Stack<int> freeID;

        public int nextMax;

        EntityBaseData()
        {
            Entities = new EntityBase[40];
            nextMax = 1;
            freeID = new Stack<int>(25);
        }
    }
}
