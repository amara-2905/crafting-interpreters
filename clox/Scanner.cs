public class Scanner{    
    public static Scanner scanner;
    public string source;
    public int Start; 
    public int Current; 
    public int Line;

    public Scanner(){}

    public void InitScanner(string source){
        scanner = this;
        this.source = source;
        Start = 0;
        Current = 0;
        Line = 1;
    }

    public static Token ScanToken(){
        SkipWhiteSpace();
        scanner.Start = scanner.Current;
        if (IsAtEnd()) return MakeToken(TokenType.TOKEN_EOF);
        char c = Advance();
        if (IsAlpha(c)) return Identifier();
        if (IsDigit(c)) return Number();
        switch (c){
            case '(' : return MakeToken(TokenType.TOKEN_LEFT_PAREN);
            case ')' : return MakeToken(TokenType.TOKEN_RIGHT_PAREN);
            case '{' : return MakeToken(TokenType.TOKEN_LEFT_BRACE);
            case '}' : return MakeToken(TokenType.TOKEN_RIGHT_BRACE);
            case ';' : return MakeToken(TokenType.TOKEN_SEMICOLON);
            case ',' : return MakeToken(TokenType.TOKEN_COMMA);
            case '.' : return MakeToken(TokenType.TOKEN_DOT);
            case '-' : return MakeToken(TokenType.TOKEN_MINUS);
            case '+' : return MakeToken(TokenType.TOKEN_PLUS);
            case '/' : return MakeToken(TokenType.TOKEN_SLASH);
            case '*' : return MakeToken(TokenType.TOKEN_STAR);
            case '!' : return MakeToken(Match('=') ?TokenType.TOKEN_BANG_EQUAL : TokenType.TOKEN_BANG);
            case '=' : return MakeToken(Match('=') ?TokenType.TOKEN_EQUAL_EQUAL : TokenType.TOKEN_EQUAL);
            case '<' : return MakeToken(Match('=') ?TokenType.TOKEN_LESS_EQUAL : TokenType.TOKEN_LESS);
            case '>' : return MakeToken(Match('=') ?TokenType.TOKEN_GREATER_EQUAL : TokenType.TOKEN_GREATER);
            case '"' : return String();
        }
        return ErrorToken("Unexpected character.");
    }

    public static bool IsAlpha(char c){
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    public static Token Identifier(){
        while (IsAlpha(Peek()) || IsDigit(Peek())) Advance();
        return MakeToken(IdentifierType());
    }

    static TokenType IdentifierType(){
        switch (scanner.source[scanner.Start]){
            case 'a' : return CheckKeyword(1, 2, "nd", TokenType.TOKEN_AND);
            case 'c' : return CheckKeyword(1, 4, "lass", TokenType.TOKEN_CLASS);
            case 'e' : return CheckKeyword(1, 3, "lse",TokenType.TOKEN_ELSE);
            case 'i' : return CheckKeyword(1, 1, "f",TokenType.TOKEN_IF);
            case 'n' : return CheckKeyword(1, 2, "il",TokenType.TOKEN_NIL);
            case 'o' : return CheckKeyword(1, 1, "r", TokenType.TOKEN_OR);
            case 'p' : return CheckKeyword(1, 4, "rint", TokenType.TOKEN_PRINT);
            case 'r' : return CheckKeyword(1, 5, "eturn", TokenType.TOKEN_RETURN);
            case 's' : return CheckKeyword(1, 4, "uper", TokenType.TOKEN_SUPER);
            case 't' : 
                if (scanner.Current - scanner.Start > 1){
                    switch (scanner.source[scanner.Start + 1]){
                        case 'h': return CheckKeyword(2, 2, "is", TokenType.TOKEN_THIS);
                        case 'r': return CheckKeyword(2, 2, "ue", TokenType.TOKEN_TRUE);
                    }
                }
                break;
            case 'v': return  CheckKeyword(1, 2, "ar", TokenType.TOKEN_VAR);
            case 'w': return  CheckKeyword(1, 4, "hile", TokenType.TOKEN_WHILE);
        }
        return TokenType.TOKEN_IDENTIFIER;
    }

    static TokenType CheckKeyword(int Start, int Length, string rest, TokenType type){
        if (scanner.Current - scanner.Start == Start + Length && scanner.source.Substring(scanner.Start + Start, Length) == rest){
            return type;
        }
        return TokenType.TOKEN_IDENTIFIER;
    }

    public static bool IsDigit(char c){
        return c >= '0' && c <= '9';
    }

    public static Token Number(){
        while (IsDigit(Peek())) Advance();
        if (Peek() == '.' && IsDigit(PeekNext())){
            Advance();
            while (IsDigit(Peek())) Advance();
        }
        return MakeToken(TokenType.TOKEN_NUMBER);
    }

    public static Token String(){
        while (Peek() != '"' && !IsAtEnd()){
            if (Peek() == '\n') scanner.Line++;
            Advance();
        }
        if (IsAtEnd()) return ErrorToken("Unterminated String");
        Advance();
        return MakeToken(TokenType.TOKEN_STRING);
    }

    public static void SkipWhiteSpace(){
        for (; ; ){
            char c = Peek();
            switch (c){
                case ' '  : 
                case '\r' :
                case '\t' :
                    Advance();
                    break;
                case '\n' :
                    scanner.Line++;
                    Advance();
                    break;
                case '/'  :
                    if (PeekNext() == '/'){
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else{
                        return;
                    }
                    break;
                default:
                    return;
            }
        }
    }

    public static char PeekNext(){
        if (scanner.Current + 1 >= scanner.source.Length) return '\0';
        return scanner.source[scanner.Current + 1];
    }

    public static char Peek(){
        if (IsAtEnd()) return '\0';
        return scanner.source[scanner.Current];
    }

    public static bool Match(char expected){
        if (IsAtEnd()) return false;
        if (scanner.source[scanner.Current] != expected) return false;
        scanner.Current++;
        return true;
    }

    public static char Advance(){
        return scanner.source[scanner.Current++];
    }

    public static bool IsAtEnd(){
        return scanner.Current >= scanner.source.Length;
    }

    public static Token MakeToken(TokenType type){
        Token token = new Token();
        token.type = type;
        token.Start = scanner.Start;
        token.Length = (int)(scanner.Current - scanner.Start);
        token.line = scanner.Line;
        return token;
    }

    public static Token ErrorToken(string message){
        Token token = new Token();
        token.type = TokenType.TOKEN_ERROR;
        token.Start = -1;
        token.Length = message.Length;
        token.line = scanner.Line;
        return token;
    }

}

public class Token{
    public TokenType type;
    public int Start;
    public int Length;
    public int line;
}

public enum TokenType{
    // Single-character tokens.
    TOKEN_LEFT_PAREN, TOKEN_RIGHT_PAREN,
    TOKEN_LEFT_BRACE, TOKEN_RIGHT_BRACE,
    TOKEN_COMMA, TOKEN_DOT, TOKEN_MINUS, TOKEN_PLUS,
    TOKEN_SEMICOLON, TOKEN_SLASH, TOKEN_STAR,
    // One or two character tokens.
    TOKEN_BANG, TOKEN_BANG_EQUAL,
    TOKEN_EQUAL, TOKEN_EQUAL_EQUAL,
    TOKEN_GREATER, TOKEN_GREATER_EQUAL,
    TOKEN_LESS, TOKEN_LESS_EQUAL,
    // Literals.
    TOKEN_IDENTIFIER, TOKEN_STRING, TOKEN_NUMBER,
    // Keywords.
    TOKEN_AND, TOKEN_CLASS, TOKEN_ELSE, TOKEN_FALSE,
    TOKEN_FOR, TOKEN_FUN, TOKEN_IF, TOKEN_NIL, TOKEN_OR,
    TOKEN_PRINT, TOKEN_RETURN, TOKEN_SUPER, TOKEN_THIS,
    TOKEN_TRUE, TOKEN_VAR, TOKEN_WHILE,

    TOKEN_ERROR, TOKEN_EOF
}