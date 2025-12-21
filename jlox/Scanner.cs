public class Scanner{
    private readonly string source;
    private readonly List<Token> tokens = new();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string source){
        this.source = source;
    }

    public List<Token> ScanTokens(){
        while (!IsAtEnd()){
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private bool IsAtEnd(){
        return current >= source.Length;
    }

    private void ScanToken()
    {
        char c = advance();
        switch (c){
            case '(': addToken(TokenType.LEFT_PAREN); break;
            case ')': addToken(TokenType.RIGHT_PAREN); break;
            case '{': addToken(TokenType.LEFT_BRACE); break;
            case '}': addToken(TokenType.RIGHT_BRACE); break;
            case ',': addToken(TokenType.COMMA); break;
            case '.': addToken(TokenType.DOT); break;
            case '-': addToken(TokenType.MINUS); break;
            case '+': addToken(TokenType.PLUS); break;
            case ';': addToken(TokenType.SEMICOLON); break;
            case '*': addToken(TokenType.STAR); break; 
            case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '/':
                if (match('/')) {
                    while (peek() != '\n' && !IsAtEnd()) advance();
                } else {
                    addToken(TokenType.SLASH);
                }
                break;
            default:
                Lox.Error(line,"Unexpected Character.");
                break;
        }
    }

    private char advance(){
        return source[current++];
    }

    private void addToken(TokenType type){
        addToken(type, null);
    }

    private void addToken(TokenType type, object literal){
        string text = source.Substring(start,current);
        tokens.Add(new Token(type, text, literal, line));
    }

    private bool match(char expected){
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false; 
        current++;
        return true;
    }

    private char peek(){
        if (IsAtEnd()) return '\0';
        return source[current];
    }
}