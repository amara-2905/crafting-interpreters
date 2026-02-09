public class Scanner{    
    public Scanner scanner;
    public string Start; 
    public string Current; 
    public int Line;

    public Scanner(){}

    public void InitScanner(string source){
        scanner.Start = source;
        scanner.Current = source;
        scanner.Line = 1;
    }
}