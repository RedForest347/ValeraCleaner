using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using System.Reflection;
using System;
using Object = UnityEngine.Object;

namespace RangerV
{
    public class PoolManager : MonoBehaviour
    {
        #region Options

        const int max_new_elements_per_frame = 3;

        #endregion Options

        static Dictionary<int, PoolCellData> poolDictionary = new Dictionary<int, PoolCellData>();
        static PoolManager pool_manager;

        public EntityBase[] ObjectsToStartPooling;

        public static PoolManager poolManager
        {
            get
            {
                if (pool_manager == null)
                    pool_manager = FindObjectOfType<PoolManager>();

                return pool_manager;
            }
            private set { }
        }

        private void Update()
        {
            int new_elements_per_this_frame = 0;

            foreach (PoolCellData poolCellData in poolDictionary.Values)
            {
                if (new_elements_per_this_frame >= max_new_elements_per_frame)
                    break;

                if (poolCellData.optimal_pool_count > poolCellData.PoolObjectStack.Count)
                {
                    poolCellData.CreateNewElements(1);
                    new_elements_per_this_frame++;
                }
            }
        }

        public static GameObject InstantiateS(GameObject prefab)
        {
            return InstantiateS(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public static GameObject InstantiateS(GameObject prefab, Vector3 position)
        {
            return InstantiateS(prefab, position, Quaternion.identity, null);
        }

        public static GameObject InstantiateS(GameObject prefab, Vector3 position, Quaternion rotation, Transform Parent = null)
        {
            GameObject OutObject;

            if (!poolDictionary.ContainsKey(prefab.GetInstanceID()))
            {
                OutObject = Instantiate(prefab, position, rotation);
                EntityBase entityBase = OutObject.GetComponent<EntityBase>();
                PoolComponent poolComponent = entityBase?.GetCmp<PoolComponent>();

                if (poolComponent != null)
                {
                    poolComponent.id = prefab.GetInstanceID();
                    CreatePoolCell(prefab.GetComponent<EntityBase>(), entityBase, prefab.GetInstanceID(), poolComponent.start_pool_size);
                }
                return OutObject;
            }
            else
            {
                return poolDictionary[prefab.GetInstanceID()].GetFromPool(position, rotation, Parent);
            }
        }

        static void CreatePoolCell(EntityBase prefabEntityBase, EntityBase createdEntityBase, int pool_id, int optimal_pool_count)
        {
            if (pool_id == 0)
            {
                Debug.LogError("попытка создать пул с ключем \"0\" объект = " + prefabEntityBase.name + ". объект добавлен в пул не будет");
                return;
            }
            GameObject poolHolder = new GameObject(prefabEntityBase.name + " pool");
            poolHolder.transform.parent = poolManager.transform;
            poolDictionary.Add(pool_id, new PoolCellData(poolHolder, prefabEntityBase, createdEntityBase, optimal_pool_count));
        }

        public static void DestroyS(GameObject ToDestroy)
        {
            DestroyS(ToDestroy.GetComponent<EntityBase>());
        }

        public static void DestroyS(EntityBase entityBase)
        {
            if (entityBase?.GetCmp<PoolComponent>() != null)
            {
                int pool_id = entityBase.GetCmp<PoolComponent>().id;

                if (poolDictionary.ContainsKey(pool_id))
                {
                    poolDictionary[pool_id].ReturnToPool(entityBase);
                    return;
                }
            }

            entityBase.OnDeactivate();
            Destroy(entityBase.gameObject);
        }
    }

    class PoolCellData
    {
        public Stack<PoolObjectData> PoolObjectStack { get; private set; }
        public int optimal_pool_count { get; private set; }
        ReflectionData[] reflectionData;
        EntityBase prefabEntityBase;
        Transform CellParent;
        GameObject Prefab;
        int pool_id;

        public PoolCellData(GameObject CellParent, EntityBase PrefabEntityBase, EntityBase CreatedEntityBase, int optimal_pool_count)
        {
            PoolObjectStack = new Stack<PoolObjectData>();
            this.optimal_pool_count = optimal_pool_count;
            this.CellParent = CellParent.transform;
            prefabEntityBase = PrefabEntityBase;
            Prefab = PrefabEntityBase.gameObject;
            pool_id = Prefab.GetInstanceID();
            reflectionData = CreateReflectionData(CreatedEntityBase);
        }

        public void ReturnToPool(EntityBase entityBase)
        {
            PoolObjectData poolObject = new PoolObjectData(CellParent.transform, entityBase);
            SetDefoultComponents(entityBase);
            poolObject.SetNullParam();
            PoolObjectStack.Push(poolObject);
        }

        public void CreateNewElements(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newObject = Object.Instantiate(Prefab);
                EntityBase entityBase = newObject.GetComponent<EntityBase>();
                entityBase.GetCmp<PoolComponent>().id = Prefab.GetInstanceID();
                PoolObjectData poolObject = new PoolObjectData(CellParent.transform, entityBase);
                poolObject.SetNullParam();
                PoolObjectStack.Push(poolObject);
            }
        }

        public GameObject GetFromPool(Vector3 position, Quaternion rotation, Transform Parent)
        {
            if (PoolObjectStack.Count > 0)
            {
                PoolObjectData obj = PoolObjectStack.Pop();
                obj.SetNewParam(position, rotation, Parent);
                obj.thisEntityBase.Awake();
                return obj.thisGameObject;
            }
            else
            {
                GameObject obj = Object.Instantiate(Prefab, position, rotation, Parent);
                obj.GetComponent<EntityBase>().GetCmp<PoolComponent>().id = Prefab.GetInstanceID();
                optimal_pool_count++;
                return obj;
            }
        }

        ReflectionData[] CreateReflectionData(EntityBase entityBase)
        {
            //entityBase.GetAllComponents(out List<ComponentBase> components, out List<Type> types);
            List<ComponentBase> components = entityBase.GetAllComponents();
            //List<Type> types;

            FieldInfo[] fields = new FieldInfo[0];
            ReflectionData[] reflectionData = new ReflectionData[components.Count];
            object[] values;

            for (int i = 0; i < components.Count; i++)
            {
                Type type = components[i].GetType();
                fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(n => Attribute.IsDefined(n, typeof(PoolAttribute))).ToArray();

                values = new object[fields.Length];
                for (int j = 0; j < fields.Length; j++)
                {
                    values[j] = fields[j].GetValue(components[i]);
                }
                reflectionData[i] = new ReflectionData(type, fields, values);
            }

            return reflectionData;
        }

        void SetDefoultComponents(object o)
        {
            SetDefoultComponents((EntityBase)o);
        }

        void SetDefoultComponents(EntityBase entityBase)
        {
            List<Type> listType = new List<Type>();
            //entityBase.GetAllComponents(out List<ComponentBase> components, out List<Type> componentTypes);
            List<ComponentBase> components = entityBase.GetAllComponents();

            for (int i = 0; i < reflectionData.Length; i++)
                listType.Add(reflectionData[i].ComponentType);

            int sum1 = 0;
            int sum2 = 0;

            if (components.Count == listType.Count)
            {
                for (int i = 0; i < listType.Count; i++)
                {
                    sum1 += components[i].GetType().GetHashCode();
                    sum2 += listType[i].GetHashCode();
                }
            }

            if (sum1 != sum2 || sum1 == 0)
            {
                for (int i = 0; i < components.Count; i++)
                    if (!listType.Contains(components[i].GetType()))
                        entityBase.RemoveCmp(components[i].GetType());

                //entityBase.GetAllComponents(out components, out componentTypes);
                
            }

            components = entityBase.GetAllComponents();
            List<Type> types = new List<Type>();

            for (int i = 0; i < components.Count; i++)
                types.Add(components[i].GetType());

            for (int i = 0; i < reflectionData.Length; i++)
            {
                int length = reflectionData[i].fieldInfos.Length;

                for (int k = 0; k < length; k++)
                {
                    reflectionData[i].fieldInfos[k].SetValue(components[types.IndexOf(reflectionData[i].ComponentType)], reflectionData[i].values[k]);
                }
            }
        }
    }

    class PoolObjectData
    {
        public GameObject thisGameObject;
        public EntityBase thisEntityBase;
        Transform thisParent;
        Transform transform;

        public PoolObjectData(Transform Parent, GameObject gameObject)
        {
            thisEntityBase = gameObject.GetComponent<EntityBase>();
            this.thisGameObject = gameObject;
            transform = gameObject.transform;
            this.thisParent = Parent;
        }

        public PoolObjectData(Transform Parent, EntityBase entityBase)
        {
            thisEntityBase = entityBase;
            this.thisGameObject = entityBase.gameObject;
            transform = thisGameObject.transform;
            this.thisParent = Parent;
        }

        public void SetNullParam()
        {
            thisEntityBase.OnDeactivate();
            thisGameObject.SetActive(false);
            thisGameObject.transform.parent = thisParent;
        }

        public void SetNewParam(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            thisGameObject.transform.position = position;
            thisGameObject.transform.rotation = rotation;
            transform.parent = parent;
            thisGameObject.SetActive(true);
        }
    }

    class ReflectionData
    {
        public Type ComponentType;
        //public int Count;
        public FieldInfo[] fieldInfos;
        public object[] values;

        public ReflectionData(Type ComponentType, FieldInfo[] fieldInfos, object[] values)
        {
            this.ComponentType = ComponentType;
            this.fieldInfos = fieldInfos;
            this.values = values;
            //Count = fieldInfos.Length;
        }
    }
}