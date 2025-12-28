using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class GenerateAst{
    public static void Main(string[] args){
        if (args.Length != 1){
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
        }
        string outputDir = args[0];
        DefineAst(outputDir, "Expr", new List<string>{
            "Binary   : Expr left, Token op, Expr right",
            "Grouping : Expr expression",
            "Literal  : object value",
            "Unary    : Token op, Expr right"});
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types){
        string path = Path.Combine(outputDir, baseName + ".cs");
        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8)){
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine($"abstract class {baseName}");
            writer.WriteLine("{");
            foreach (string type in types){
                string className = type.Split(':')[0].Trim();
                string fields = type.Split(':')[1].Trim();
                DefineType(writer, baseName, className, fields);
            }
            writer.WriteLine("}");
        }
    }

    private static void DefineType(StreamWriter writer,string baseName,string className,string fieldList){
        writer.WriteLine($"    class {className} : {baseName}");
        writer.WriteLine("    {");
        string[] fields = fieldList.Split(", ");
        foreach (string field in fields){
            writer.WriteLine($"        public readonly {field};");
        }
        writer.WriteLine();
        writer.WriteLine($"        public {className}({fieldList})");
        writer.WriteLine("        {");
        foreach (string field in fields){
            string name = field.Split(' ')[1];
            writer.WriteLine($"            this.{name} = {name};");
        }
        writer.WriteLine("        }");
        writer.WriteLine("    }");
        writer.WriteLine();
    }
}
