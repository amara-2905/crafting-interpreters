public class GenerateAst{
    public static void Main(string[] args){
        try{
            if(args.Length != 1){
                Console.Error.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            string outputDir = args[0];
            DefineAst(outputDir,"Expr",new List<string> {
                        "Binary   : Expr left, Token operator, Expr right",
                        "Grouping : Expr expression",
                        "Literal  : Object value",
                        "Unary    : Token operator, Expr right"});
        }
        catch (IOException){
            throw;
        }
    }

    private static void DefineAst(string outputDir, List<string> types){
        try{
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine($"abstract class {baseName}");
            foreach(string type in types){
                string className = type.Split(":")[0].Trim;
                string fields = type.Split(":")[1].Trim;
                DefineType(writer,baseName,className,fields);
            }
            writer.WriteLine("{");

            writer.WriteLine("}");
        }
        catch (IOException){
            throw;
        }
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList){
        writer.WriteLine("  static class " + className + " extends " +baseName + " {");
        writer.WriteLine("    " + className + "(" + fieldList + ") {");
        string[] fields = fieldList.Split(", ");
        foreach(string field in fields){
            string name = field.Split(" ")[1];
            writer.WriteLine("      this." + name + " = " + name + ";");
        }
        writer.WriteLine("    }");
        writer.WriteLine();
        foreach(string field in fields){
            writer.WriteLine("    final " + field + ";");
        }
        writer.WriteLine("  }");
    }
}