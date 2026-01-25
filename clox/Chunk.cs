public enum OpCode
{
    OP_RETURN
}

public class Chunk
{
    public int Count;
    public int Capacity;
    public byte[] Code;

    public Chunk(int Count, int Capacity, byte[] Code)
    {
        this.Capacity = Capacity;
        this.Count = Count;
        this.Code = Code;   
    }

    public void InitChunk(Chunk chunk)
    {
        chunk.Capacity = 0;
        chunk.Count = 0;
        chunk.Code = null;
    }

    public void WriteChunk(Chunk chunk, byte value)
    {
        if (chunk.Capacity < chunk.Count + 1)
        {
            int OldCapacity = chunk.Capacity;
            chunk.Capacity = Memory.GrowCapacity(OldCapacity);
            chunk.Code = Memory.GrowArray<byte>(chunk.Code, OldCapacity, chunk.Capacity);
        }
        chunk.Code[chunk.Count] = value;
        chunk.Count++;
    }
}