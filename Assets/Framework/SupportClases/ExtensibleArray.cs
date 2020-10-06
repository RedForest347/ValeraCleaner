using System;

public class ExtensibleArray<T> 
{
    static int expand_step = 10;

    T[] array;
    public int Current_size { get => array.Length; }
    

    public ExtensibleArray()
    {
        array = new T[expand_step];
    }


    public T this[int index] 
    {
        get
        {
            if (index >= array.Length)
                return default(T);
            return array[index];
        }
        set
        {
            if (index >= array.Length)
                Array.Resize(ref array, array.Length + expand_step);
            array[index] = value;
        }

    }

    public static explicit operator T[](ExtensibleArray<T> extensibleArray)
    {
        if (extensibleArray == null)
            extensibleArray = new ExtensibleArray<T>();
        return extensibleArray.array;
    }
}
