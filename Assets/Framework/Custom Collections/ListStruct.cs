using System.Collections;
using System.Collections.Generic;

namespace RangerV
{
    /*public class ListStruct<T> : IList<T> where T : struct
    {
        IList<T> iList;

        public ListStruct()
        {
            if (typeof(T) == typeof(int))
                iList = (IList<T>)new ListInt<int>();
            else if (typeof(T) == typeof(float))
                iList = (IList<T>)new ListFloat<float>();
            else
                iList = new List<T>();
        }

        public T this[int index] { get => iList[index]; set => iList[index] = value; }

        public int Count => iList.Count;

        public bool IsReadOnly => iList.IsReadOnly;

        public void Add(T item)
        {
            iList.Add(item);
        }

        public void Clear()
        {
            iList.Clear();
        }

        public bool Contains(T item)
        {
            return iList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            iList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return iList.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return iList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            iList.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return iList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            iList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return iList.GetEnumerator();
        }
    }*/
}
