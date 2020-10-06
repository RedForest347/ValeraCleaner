using System;
using System.Collections;
using Debug = UnityEngine.Debug;

namespace RangerV
{
    /*public class ListClass<T> where T : class
    {
        const int start_length_of_array = 25;
        T[] array;
        int current_length;

        public int Count
        {
            get
            {
                return current_length;
            }
        }

        public ListClass()
        {
            array = new T[start_length_of_array];
            current_length = 0;
        }

        public ListClass(int start_length)
        {
            array = new T[start_length];
            current_length = 0;
        }

        public T this[int index]
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

        public void Add(T item)
        {
            if (current_length >= array.Length)
                AddRange();

            array[current_length++] = item;
        }

        void AddRange()
        {
            T[] new_array = new T[array.Length + array.Length];

            for (int i = 0; i < array.Length; i++)
                new_array[i] = array[i];

            array = new_array;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < current_length; i++)
                if (array[i] == item)
                    return true;

            return false;
        }

        public T Find(T item)
        {
            if (item != null)
            {
                for (int i = 0; i < current_length; i++)
                    if (array[i] == item)
                        return array[i];

                return null;
            }
            else
            {
                for (int i = 0; i < current_length; i++)
                    if (array[i] == null)
                        return array[i];
                return null;
            }
        }

        public bool Remove(T item)
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

        public void RemoveRange(int from, int to)
        {
            if ((from < 0 || from >= current_length) || (to < 0 || to >= current_length))
                throw new IndexOutOfRangeException();
            if (to - from < 0)
                throw new Exception("неверное значение переменных (from, to)");

            int offset = 1 + to - from;

            for (int i = from; i < current_length - offset; i++)
                array[i] = array[i + offset];

            current_length -= offset;
        }

        public T RemoveLast()
        {
            if (current_length == 0)
                throw new IndexOutOfRangeException();

            return array[current_length--];
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < current_length; i++)
                if (array[i] == item)
                    return i;

            return -1;
        }

        public void ShowInfo()
        {
            for (int i = 0; i < current_length; i++)
            {
                Debug.Log(i + ") = " + array[i].ToString());
            }
        }

        public IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }
    }*/
}
