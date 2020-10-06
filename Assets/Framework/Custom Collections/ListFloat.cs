using System;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace RangerV
{
    public class ListFloat<T> : IList<float> where T : struct
    {
        const int start_length_of_array = 25;

        float[] array;
        int current_length;
        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                return current_length;
            }
        }

        public ListFloat()
        {
            array = new float[start_length_of_array];
            current_length = 0;
        }

        public ListFloat(int start_length)
        {
            array = new float[start_length];
            current_length = 0;
        }

        public float this[int index]
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

        public void Add(float item)
        {
            if (current_length >= array.Length)
                AddRange();

            array[current_length++] = item;
        }

        void AddRange()
        {
            float[] new_array = new float[array.Length + array.Length];

            for (int i = 0; i < current_length; i++)
                new_array[i] = array[i];

            array = new_array;
        }

        public bool Contains(float value)
        {
            for (int i = 0; i < current_length; i++)
                if (array[i] == value)
                    return true;

            return false;
        }

        public bool Remove(float item)
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

        public int IndexOf(float item)
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

        public void Insert(int index, float item)
        {
            Debug.LogError("Insert не поддерживается");
        }

        public void Clear()
        {
            Debug.LogError("Clear не поддерживается");
        }

        public void CopyTo(float[] array, int arrayIndex)
        {
            Debug.LogError("CopyTo не поддерживается");
        }

        IEnumerator<float> IEnumerable<float>.GetEnumerator()
        {
            return array.GetEnumerator() as IEnumerator<float>;
        }
    }
}
