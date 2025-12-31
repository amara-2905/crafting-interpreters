// lox.cs
using System.ComponentModel;
using System.Text;
public class Lox{
    private static readonly Interpreter interpreter = new Interpreter();
    static bool hadError = false;
    static bool hadRuntimeError = false;
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
    // To test Ast Code
    // public static void Main(string[] args)
    // {
    //     Expr Expression = new Expr.Binary(new Expr.Unary(new Token(TokenType.MINUS,"-",null, 1), new Expr.Literal(123)),new Token(TokenType.STAR,"*",null,1),new Expr.Grouping(new Expr.Literal(45.67)));
    //     Console.WriteLine(new AstPrinter().Print(Expression));
    // }

    private static void RunFile(string Path){
        try{
            byte[] bytes = File.ReadAllBytes(Path);
            Run(Encoding.Default.GetString(bytes));
            if (hadError){
               Environment.Exit(65); 
            }
            if (hadRuntimeError){
                Environment.Exit(70);
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
                string? line = reader.ReadLine();
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
        Parser parser = new Parser(tokens);
        List<Stmt> Statements = parser.Parse();
        if (hadError) return;
        interpreter.Interpret(Statements);
    }

    public static void Error(int line, string message){
        Report(line,"",message);
    }

    private static void Report(int line, string where, string message){
        Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        hadError = true;
    }

    public static void Error(Token token, string message){
        if (token.type == TokenType.EOF){
            Report(token.line, " at end", message);
        }
        else{
            Report(token.line, " at '" + token.lexeme + "'", message);
        }
    }

    public static void RuntimeError(RuntimeError error){
        Console.Error.WriteLine(error.Message + "\n[line " + error.token.line + "]");
        hadRuntimeError = true;
    } 
}