class Lox{
    public static void Main(string[] args){
        VirtualMachine.InitVM();
        Chunk chunk = new Chunk(0, 0, null);
        chunk.InitChunk(chunk);
        int Constant = chunk.AddConstant(chunk,-1.2);
        chunk.WriteChunk(chunk,(byte)OpCode.OP_CONSTANT,123);
        Constant = chunk.AddConstant(chunk,3.4);
        chunk.WriteChunk(chunk,(byte)OpCode.OP_CONSTANT,123);
        chunk.WriteChunk(chunk,(byte)OpCode.OP_ADD,123);
        chunk.WriteChunk(chunk,(byte)Constant,123);
        chunk.WriteChunk(chunk,(byte)OpCode.OP_NEGATE,123);
        chunk.WriteChunk(chunk, (byte)OpCode.OP_RETURN,123);
        Debug.DisassembleChunk(chunk,"test chunk");
        VirtualMachine.FreeVM();
        chunk.FreeChunk(chunk);
    }
}
