// lox.cs
using System.Text;
using System.IO;

public class Lox
{
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }
        catch (IOException)
        {
            throw;
        }
    }

    private static void RunFile(string Path)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(Path);
            Run(Encoding.Default.GetString(bytes));
        }
        catch (IOException)
        {
            throw;
        }
        
    }

    private static void RunPrompt()
    {
        try
        { 
            StreamReader reader = new StreamReader(Console.OpenStandardInput());
            for ( ; ; )
            {
                Console.WriteLine("> ");
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                Run(line);
            }
        }
        catch (IOException)
        {
            throw;
        }
    }

    private static void Run(string Source)
    {
        
    }
}