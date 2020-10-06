using System;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace RangerV
{
    public class ListInt<T> : IList<int> where T : struct
    {
        const int start_length_of_array = 25;

        int[] array;
        int current_length;
        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                return current_length;
            }
        }

        public ListInt()
        {
            array = new int[start_length_of_array];
            current_length = 0;
        }

        public ListInt(int start_length)
        {
            array = new int[start_length];
            current_length = 0;
        }

        public int this[int index]
        {
            get
            {
                return array[index];
            }
            set
            {
                array[index] = value;
            }
        }

        public void Add(int item)
        {
            if (current_length >= array.Length)
                AddRange();

            array[current_length++] = item;
        }

        void AddRange()
        {
            int[] new_array = new int[array.Length + array.Length];

            for (int i = 0; i < current_length; i++)
                new_array[i] = array[i];

            array = new_array;
        }

        public bool Contains(int value)
        {
            for (int i = 0; i < current_length; i++)
                if (array[i] == value)
                    return true;

            return false;
        }

        public bool Remove(int item)
        {
            for (int i = 0; i < current_length; i++)
            {
                if (array[i] == item)
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= current_length)
                throw new IndexOutOfRangeException();

            current_length--;
            if (index < current_length)
            {
                System.Array.Copy(array, index + 1, array, index, current_length - index);
            }
        }

        public int IndexOf(int item)
        {
            for (int i = 0; i < current_length; i++)
                if (array[i] == item)
                    return i;

            return -1;
        }

        public IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }

        public void Insert(int index, int item)
        {
            Debug.LogError("Insert не поддерживается");
        }

        public void Clear()
        {
            Debug.LogError("Clear не поддерживается");
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            Debug.LogError("CopyTo не поддерживается");
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return array.GetEnumerator() as IEnumerator<int>;
        }
    }
}
