using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RangerV.ThreadManager;

namespace RangerV
{
    public class CorutineManager : ProcessingBase, ICustomFixedUpdate, ICustomUpdate
    {
        static List<IEnumerator> corutines_update;
        static List<IEnumerator> corutines_fixed_update;

        static bool was_init;

        static CorutineManager()
        {
            Init();
        }

        static void Init()
        {
            if (!was_init)
            {
                corutines_update = new List<IEnumerator>();
                corutines_fixed_update = new List<IEnumerator>();
                was_init = true;
            }
        }

        public void CustomUpdate()
        {
            EnumerationUpdate();
        }

        public void CustomFixedUpdate()
        {
            EnumerationFixedUpdate();
        }

        public static void StartCorutine(IEnumerator corutine)
        {
            corutines_update.Add(corutine);
        }

        public static void StartCorutine(IEnumerable corutine)
        {
            corutines_update.Add(corutine.GetEnumerator());
        }

        public static void StartCorutineFixedUpdate(IEnumerator corutine)
        {
            corutines_fixed_update.Add(corutine);
        }

        public static void StartCorutineFixedUpdate(IEnumerable corutine)
        {
            corutines_fixed_update.Add(corutine.GetEnumerator());
        }


        static void EnumerationUpdate()
        {
            for (int i = 0; i < corutines_update.Count; i++)
                if (!corutines_update[i].MoveNext())
                    corutines_update.RemoveAt(i);
        }

        static void EnumerationFixedUpdate()
        {
            for (int i = 0; i < corutines_fixed_update.Count; i++)
                if (!corutines_fixed_update[i].MoveNext())
                    corutines_fixed_update.RemoveAt(i);
        }

        public static IEnumerable WaitCompleteFunction(ThreadSStart Func)
        {
            TaskInfo taskInfo = new TaskInfo(Func);
            while (!taskInfo.is_complete)
                yield return null;
        }

        public static IEnumerable WaitCompleteFunction(ParameterizedThreadSStart Func, object o)
        {
            TaskInfo taskInfo = new TaskInfo(Func, o);
            while (!taskInfo.is_complete)
                yield return null;
        }

        public static void RemoveAll()
        {
            corutines_fixed_update = new List<IEnumerator>();
        }
    }
}
