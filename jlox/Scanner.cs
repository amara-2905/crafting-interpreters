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
            case ' ':
            case '\r':
            case '\t':break;
            case '\n': line++; break;
            case '"': String(); break;
            case 'o':
                if (match('\r')){
                    addToken(TokenType.OR);
                }
                break;
            default:
                if (IsDigit(c)) {
                    Number();
                } else if (IsAlpha(c)){
                    Identifier();
                } 
                else {
                    Lox.Error(line, "Unexpected character.");
                }
                break;
        }
    }

    private char advance(){
        return source[current++];
    }

    private void addToken(TokenType type){
        addToken(type, null);
    }

    private void addToken(TokenType type, object? literal){
        string text = source.Substring(start,current - start);
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
      
    private void String(){
        while (peek() != '"' && !IsAtEnd()) {
            if (peek() == '\n') line++;
            advance();
        }
        if (IsAtEnd()) {
            Lox.Error(line, "Unterminated string.");
            return;
        }
        advance();
        string value = source.Substring(start + 1, current - 1 - start);
        addToken(TokenType.STRING, value);
    }

    private bool IsDigit(char c){
        return c >= '0' && c <= '9';
    }

    private void Number(){
        while(IsDigit(peek())) advance();
        if (peek() == '.' && IsDigit(PeekNext())){
            advance();
            while (IsDigit(peek())) advance();
        }
        addToken(TokenType.NUMBER,double.Parse(source.Substring(start, current - start)));
    }

    private char PeekNext(){
        if (current + 1 >= source.Length) return'\0';
        return source[current + 1];
    }

    private void Identifier(){
        while(IsAlphaNumeric(peek())) advance();
        string text = source.Substring(start, current - start);
        TokenType type = keywords.TryGetValue(text, out var t) ? t : TokenType.IDENTIFIER;
        addToken(type);
    }

    private bool IsAlpha(char c){
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '-';
    }

    private bool IsAlphaNumeric(char c){
        return IsAlpha(c) || IsDigit(c);
    }

    private static readonly Dictionary<string, TokenType> keywords =
        new()
        {
            ["and"]    = TokenType.AND,
            ["class"]  = TokenType.CLASS,
            ["else"]   = TokenType.ELSE,
            ["false"]  = TokenType.FALSE,
            ["for"]    = TokenType.FOR,
            ["fun"]    = TokenType.FUN,
            ["if"]     = TokenType.IF,
            ["nil"]    = TokenType.NIL,
            ["or"]     = TokenType.OR,
            ["print"]  = TokenType.PRINT,
            ["return"] = TokenType.RETURN,
            ["super"]  = TokenType.SUPER,
            ["this"]   = TokenType.THIS,
            ["true"]   = TokenType.TRUE,
            ["var"]    = TokenType.VAR,
            ["while"]  = TokenType.WHILE
        };
}