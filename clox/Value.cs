using Value = System.Double;
public class ValueArray{
    public int Capacity;
    public int Count;
    public Value[] Values;

    public static void InitValueArray(ValueArray array){
        array.Values = null;
        array.Capacity = 0;
        array.Count = 0;
    }

    public static void WriteValueArray(ValueArray array, Value value){
        if (array.Capacity < array.Count + 1){
            int OldCapacity = array.Capacity;
            array.Capacity = Memory.GrowCapacity(OldCapacity);
            array.Values = Memory.GrowArray<Value>(array.Values, OldCapacity, array.Capacity);
        }
        array.Values[array.Count] = value;
        array.Count++;
    }

    public static void FreeValueArray(ValueArray array){
        Memory.FreeArray<Value>(ref array.Values);
        InitValueArray(array);
    }

    public static void PrintValue(Value value)
    {
        Console.Write(value);
    }
}