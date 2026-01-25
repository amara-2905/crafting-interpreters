using System;
public static class Memory
{
    public static int GrowCapacity(int OldCapacity)
    {
        return OldCapacity < 8 ? 8 : OldCapacity * 2;
    }

    public static T[] GrowArray<T>(T[] array, int OldCount, int NewCount)
    {
        T[] NewArray = new T[NewCount];
        if (array != null)
        {
            Array.Copy(array, NewArray, OldCount);
        }
        return NewArray;
    }
}