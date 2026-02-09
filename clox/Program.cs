using System.Text;

class Lox{
    public static void Main(string[] args){
        VirtualMachine.InitVM();
        if (args.Length == 0){
            Repl();
        }
        else if (args.Length == 1){
            RunFile(args[0]);
        }
        else{
            Console.Error.WriteLine("Usage: clox [path]");
            Environment.Exit(64);
        }
        VirtualMachine.FreeVM();
    }

    public static void Repl(){
        while (true){
            string line;
            Console.Write("> ");
            line = Console.ReadLine();
            if (line == null){ 
                Console.WriteLine();
                break;
            }
            VirtualMachine.Interpret(line);
        }
    }

    public static void RunFile(string path){
        string source = ReadFile(path);
        InterpretResult result = VirtualMachine.Interpret(source);
        if (result == InterpretResult.INTERPRET_COMPILE_ERROR) Environment.Exit(65);
        if (result == InterpretResult.INTERPRET_RUNTIME_ERROR) Environment.Exit(70);
    }

    public static string ReadFile(string path){
        try{
            using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            long fileSize = fs.Length;
            byte[] buffer = new byte[fileSize]; 
            int bytesRead = fs.Read(buffer, 0, buffer.Length);
            if (bytesRead < fileSize){
                Console.Error.WriteLine($"Could not read file \"{path}\".");
                Environment.Exit(74);
            }
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        catch (IOException){
            Console.Error.WriteLine($"Could not open file \"{path}\".");
            Environment.Exit(74);
            return null!;
        }
    }
}
