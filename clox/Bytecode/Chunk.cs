public enum OpCode : byte{
    OP_RETURN
}

public class Chunk{
    public int Count;
    public int Capacity;
    public byte[] Code;

    public Chunk(){
        Count = 0;
        Capacity = 0;
        Code = null;
    }

    public void InitChunk(Chunk chunk){
        chunk.Count = 0;
        chunk.Capacity = 0;
        chunk.Code = null;
    }

    public void WriteChunk(Chunk chunk, byte value){
        if (chunk.capacity < chunk.count + 1){
            int oldCapacity = chunk.capacity;
            chunk.capacity = oldCapacity < 8 ? 8 : oldCapacity * 2;
            Array.Resize(ref chunk.code, chunk.capacity);
        }
        chunk.code[chunk.count] = value;
        chunk.count++;
    }

}
