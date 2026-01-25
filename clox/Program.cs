class Lox
{
    public static void Main()
    {
        Chunk chunk = new Chunk(0, 0, null);
        chunk.InitChunk(chunk);
        chunk.WriteChunk(chunk, 42);
        chunk.WriteChunk(chunk, 43);
        chunk.WriteChunk(chunk, 44);
        Console.WriteLine("Count: " + chunk.Count);
        Console.WriteLine("Capacity: " + chunk.Capacity);
        for (int i = 0; i < chunk.Count; i++)
        {
            Console.WriteLine("Code[" + i + "] = " + chunk.Code[i]);
        }
    }
}
