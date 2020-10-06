using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RangerV
{

    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        private static T _instance;
        public static bool isApplicationQuitting;
        protected static object _lock = new object();

        public static T Instance
        {
            get
            {

                if (isApplicationQuitting)
                    return null;
                
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject("[SINGLETON] " + typeof(T));
                            //Debug.Log("[SINGLETON] " + typeof(T).Name + " created");
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return _instance;
                }
            }
        }
    }
}
