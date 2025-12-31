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

        DefineAst(outputDir,"Stmt", new List<string>{
            "Expression : Expr expression",
            "Print : Expr expression" });
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types){
        string path = Path.Combine(outputDir, baseName + ".cs");
        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8)){
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine($"abstract class {baseName}");
            writer.WriteLine("{");
            DefineVisitor(writer,baseName,types);
            foreach (string type in types){
                string className = type.Split(':')[0].Trim();
                string fields = type.Split(':')[1].Trim();
                DefineType(writer, baseName, className, fields);
            }
            writer.WriteLine();
            writer.WriteLine("    public abstract R Accept<R>(Visitor<R> visitor);");
            writer.WriteLine("}");
        }
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types){
        writer.WriteLine("    public interface Visitor<R>");
        writer.WriteLine("{");
        foreach (string type in types)
        {
            string typeName = type.Split(":")[0].Trim();
            writer.WriteLine("    R Visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
        }
        writer.WriteLine("    }");
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
        writer.WriteLine();
        writer.WriteLine("        public override R Accept<R>(Visitor<R> visitor)");
        writer.WriteLine("        {");
        writer.WriteLine($"            return visitor.Visit{className}{baseName}(this);");
        writer.WriteLine("        }");
        writer.WriteLine("    }");
        writer.WriteLine();
    }
}
