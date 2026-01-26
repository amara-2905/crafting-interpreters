class Lox
{
    public static void Main(string[] args)
    {
        Chunk chunk = new Chunk(0, 0, null);
        chunk.InitChunk(chunk);
        int Constant = chunk.AddConstant(chunk,1.2);
        chunk.WriteChunk(chunk,(byte)OpCode.OP_CONSTANT,123);
        chunk.WriteChunk(chunk,(byte)Constant,123);
        chunk.WriteChunk(chunk, (byte)OpCode.OP_RETURN,123);
        Debug.DisassembleChunk(chunk,"test chunk");
        chunk.FreeChunk(chunk);
    }
}
