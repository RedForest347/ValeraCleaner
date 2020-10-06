using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RangerV
{
    public class ManagerUpdate : MonoBehaviour
    {
        private List<ICustomUpdate> updates = new List<ICustomUpdate>();
        private List<ICustomFixedUpdate> fixedupdates = new List<ICustomFixedUpdate>();
        private List<ICustomLateUpdate> lateupdates = new List<ICustomLateUpdate>();


        public static ManagerUpdate Instance { get => Singleton<ManagerUpdate>.Instance; }

        public static void Init()
        {

        }

        public static void Clear()
        {
            Instance.updates = new List<ICustomUpdate>();
            Instance.fixedupdates = new List<ICustomFixedUpdate>();
            Instance.lateupdates = new List<ICustomLateUpdate>();
        }


        public static void Add(object updateble)      //посылаем сюда object унаследованный от ICustomUpdate/ICustomFixedUpdate/ICustomLateUpdate
        {
            if (updateble is ICustomUpdate)
                Instance.updates.Add(updateble as ICustomUpdate);

            if (updateble is ICustomFixedUpdate)
                Instance.fixedupdates.Add(updateble as ICustomFixedUpdate);

            if (updateble is ICustomLateUpdate)
                Instance.lateupdates.Add(updateble as ICustomLateUpdate);

        }

        public static void Remove(object updateble)     //посылаем сюда object унаследованный от ICustomUpdate/ICustomFixedUpdate/ICustomLateUpdate
        {
            if (updateble is ICustomUpdate)
                Instance.updates.Remove(updateble as ICustomUpdate);

            if (updateble is ICustomFixedUpdate)
                Instance.fixedupdates.Remove(updateble as ICustomFixedUpdate);

            if (updateble is ICustomLateUpdate)
                Instance.lateupdates.Remove(updateble as ICustomLateUpdate);

        }


        private void Update()
        {
            for (var i = 0; i < updates.Count; i++)
                updates[i].CustomUpdate();
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < fixedupdates.Count; i++)
                fixedupdates[i].CustomFixedUpdate();
        }

        private void LateUpdate()
        {
            for (var i = 0; i < lateupdates.Count; i++)
                lateupdates[i].CustomLateUpdate();
        }
    }
}
