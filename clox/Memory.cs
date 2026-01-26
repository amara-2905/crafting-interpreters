public static class Memory{
    public static int GrowCapacity(int OldCapacity){
        return OldCapacity < 8 ? 8 : OldCapacity * 2;
    }

    public static T[] GrowArray<T>(T[] array, int OldCount, int NewCount){
        try{
            T[] NewArray = new T[NewCount];
            if (array != null){
                Array.Copy(array, NewArray, OldCount);
            }
            return NewArray;
        }
        catch (OutOfMemoryException){
            Environment.Exit(1);
            throw; 
        }
    }

    public static void FreeArray<T>(ref T[] array){
        array = null;
    }
}