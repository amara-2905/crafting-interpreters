// lox.cs
using System.Text;
public class Lox{
    static bool hadError = false;

    public static void Main(string[] args){
        try{
            if (args.Length > 1){
                Console.WriteLine("Usage: jlox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1){
                RunFile(args[0]);
            }
            else{
                RunPrompt();
            }
        }
        catch (IOException){
            throw;
        }
    }

    private static void RunFile(string Path){
        try{
            byte[] bytes = File.ReadAllBytes(Path);
            Run(Encoding.Default.GetString(bytes));
            if (hadError){
               Environment.Exit(65); 
            }
        }
        catch (IOException){
            throw;
        }
        
    }

    private static void RunPrompt(){
        try{ 
            StreamReader reader = new StreamReader(Console.OpenStandardInput());
            for ( ; ; ){
                Console.Write("> ");
                string line = reader.ReadLine();
                if (line == null){
                    break;
                }
                Run(line);
                hadError = false;
            }
        }
        catch (IOException){
            throw;
        }
    }

    private static void Run(string Source){
        Scanner scanner = new Scanner(Source);
        List<Token> tokens = scanner.ScanTokens();
        foreach (Token token in tokens){
            Console.Write(token);
        } 
    }

    public static void Error(int line, string message){
        Report(line,"",message);
    }

    private static void Report(int line, string where, string message){
        Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        hadError = true;
    }
}