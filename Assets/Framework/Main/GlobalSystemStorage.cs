using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;
//using System.Diagnostics;
//using System.Diagnostics;
//using NUnit.Framework;

namespace RangerV
{
    /// <summary>
    /// вопрос, как на время отключить процессинг?
    /// (просто так исключить его из Processings не вариант, т.к. исключение-включение в Dictionary имеет неприятные последствия)
    /// </summary>

    public class GlobalSystemStorage : MonoBehaviour
    {
        public static GlobalSystemStorage Instance { get => Singleton<GlobalSystemStorage>.Instance; }
        public bool debug_mod = false;

        Dictionary<Type, ProcessingBase> Processings;


        public static void Init()
        {
            Instance.Processings = new Dictionary<Type, ProcessingBase>();
        }

        public static T Add<T>() where T : ProcessingBase, new()
        {
            if (Instance.Processings.ContainsKey(typeof(T)))
            {
                Debug.LogError("Компонент " + typeof(T).Name + " уже добавлен в GlobalSystemStorage. он не может быть добавлен повторно");
                return null;
            }


            T processing = new T();
            Instance.Processings.Add(typeof(T), processing);

            if (processing is ICustomAwake)
                (processing as ICustomAwake).OnAwake();

            if (Starter.initialized)
                if (processing is ICustomStart)
                    (processing as ICustomStart).OnStart();

            return processing;
        }

        public static T Get<T>() where T : ProcessingBase
        {
            ProcessingBase resolve;
            Instance.Processings.TryGetValue(typeof(T), out resolve);
            return (T)resolve;
        }

        public static void StartAllProcessings()
        {
            foreach (Type type in Instance.Processings.Keys)
            {
                StartProcessing(type);
            }
        }

        public static void StartProcessing<T>() where T : ProcessingBase
        {
            StartProcessing(typeof(T));
        }

        public static void StartProcessing(Type processingType)
        {
            ProcessingBase processing = Instance.Processings[processingType];

            if (processing is ICustomStart)
                (processing as ICustomStart).OnStart();

            ManagerUpdate.Add(processing);
        }

        public static void StopProcessing<T>() where T : ProcessingBase
        {
            Type processingType = typeof(T);

            ProcessingBase processing = Instance.Processings[processingType];

            if (processing is IStoppable)
                (processing as IStoppable).OnStop();

            ManagerUpdate.Remove(Instance.Processings[processingType]);
        }

        public static void DisableAllProcessings()
        {
            Dictionary<Type, ProcessingBase> processings = Instance.Processings;
            ProcessingBase[] values = new ProcessingBase[processings.Count];
            processings.Values.CopyTo(values, 0);

            for (int i = 0; i < values.Length; i++)
                if (values[i] is ICustomDisable)
                    (values[i] as ICustomDisable).OnCustomDisable();

            processings = new Dictionary<Type, ProcessingBase>();
        }
    }
}
