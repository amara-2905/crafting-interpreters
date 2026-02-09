public class Compiler
{
    public static Scanner scanner = new Scanner();
    public static void Compile(string source)
    {
        scanner.InitScanner(source);
        int line = -1;
        for (; ;)
        {
            Token token = Scanner.ScanToken();
            if (token.line != line)
            {
                Console.Write($"{token.line,4} ");
                line = token.line;
            } 
            else
            {
                Console.Write("   | ");
            }

            string lexeme =
                token.Start == -1
                ? "ERROR"
                : scanner.source.Substring(token.Start, token.Length);

            Console.WriteLine($"{(int)token.type,2} '{lexeme}'");            if (token.type == TokenType.TOKEN_EOF) break;
        }
    }


}