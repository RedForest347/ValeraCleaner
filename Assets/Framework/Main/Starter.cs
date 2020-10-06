using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.SceneManagement;

namespace RangerV
{

    // В Starter выполняется главный Awake сцены. В Awake сначала создается ManagerUpdate, после происходит добавление processing'ов в GSS словарь
    // (происходит это в StarterSetup, переопределенном в специальном классе унаследованном от Starter). 
    // При добавлении в GSS словарь, на processing"е выполняется метод OnAwake (при наличии интерфейса IAwake).
    //
    // 1) создается ManagerUpdate
    // 2) происходит StarterSetup отвечающий за добавление processing'ов (именно его нужно прописать в стартере уровня)
    // 3) при добавлении на processing"е выполняется метод OnAwake
    //
    //

    public class Starter : MonoBehaviour        
    {
        public static bool initialized { get; private set; }

        private void Awake()
        {
            //initialized = false;
            //Debug.Log("initialized = " + initialized);
            ManagerUpdate.Init();
            GlobalSystemStorage.Init();
            StarterSetup();
            
            EntitiesInitializing();
            initialized = true;
            Debug.Log("initialized:   " + initialized);
        }

        private void Start()
        {
            GlobalSystemStorage.StartAllProcessings();
            
        }

        private void OnEnable()
        {
            if (!initialized)
                OnRebuild();
        }

        void OnRebuild()
        {
            ManagerUpdate.Init();
            GlobalSystemStorage.Init();
            StarterSetup();
            initialized = true;

            GlobalSystemStorage.StartAllProcessings();
            Debug.LogWarning("Произошел ребилд. При возникновении багов, опишите проблему и обратитесь к разработчику");
        }

        void EntitiesInitializing()
        {
            EntityBase[] objs = FindObjectsOfType<EntityBase>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].state.requireStarter)
                    objs[i].SetupAfterStarter();
            }
        }

        void EntitiesDeinitializing()
        {
            for (int entity = 0; entity < EntityBase.entity_count; entity++)
                if (EntityBase.ContainsEntity(entity))
                    Destroy(EntityBase.GetEntity(entity));
        }

        /// <summary>
        /// Setup стартера уровня. В стартере уровня выполняется добавление processing'ов в GSS
        /// </summary>
        public virtual void StarterSetup() { }

        private void OnApplicationQuit()
        {
            ClearSceneOnQuit();
        }

        private void OnDisable()
        {
            ClearScene();
        }

        void ClearScene()
        {
            if (!initialized)
                return;

            ManagerUpdate.Clear();
            GlobalSystemStorage.DisableAllProcessings();
            Group.Clear();
            initialized = false;
        }

        void ClearSceneOnQuit()
        {
            if (!initialized)
                return;

            EntitiesDeinitializing();
            ManagerUpdate.Clear();
            GlobalSystemStorage.DisableAllProcessings();
            Group.Clear();
            initialized = false;
        }
    }
}
