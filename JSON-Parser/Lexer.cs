public class Lexer
{
    public string _jsonString { get; }

    private int _position;
    private char _currentChar;
    private char _nextChar;

    public Lexer(string jsonString)
    {
        _jsonString = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
        _position = -2;
        _currentChar = '\0';
        _nextChar = '\0';

        Next(); // to move so we start our lexer on a clean safe state
    }

    private void ConsumeWhiteSpaces()
    {
        while (_currentChar != '\0' && char.IsWhiteSpace(_currentChar))
        {
            Next();
        }
    }

    private void Next()
    {
        _position++;
        _currentChar = _nextChar;

        if (_position <= (_jsonString.Length - 2))
        {
            _nextChar = _jsonString[_position + 1];
        }
        else
        {
            _nextChar = '\0';
        }
    }

    public Token Lex()
    {
        Next();
        ConsumeWhiteSpaces();
        var currentChar = _currentChar;
        var charString = currentChar.ToString();

        if (currentChar == '\0') return new Token(TokenKind.EOF, charString);
        else if (currentChar == '{') return new Token(TokenKind.OpenCurlyBrace, charString);
        else if (currentChar == '}') return new Token(TokenKind.CloseCurlyBrace, charString);
        else if (currentChar == ',') return new Token(TokenKind.Comma, charString);
        else if (currentChar == ':') return new Token(TokenKind.Colon, charString);
        else if (currentChar == '[') return new Token(TokenKind.OpenBracket, charString);
        else if (currentChar == ']') return new Token(TokenKind.CloseBracket, charString);
        else if (currentChar == '"') return MakeStringLiteral();
        else if (char.IsDigit(currentChar)) return MakeNumericLiteral();
        else if (currentChar == 't' || currentChar == 'f') return MakeBooleanLiteral();
        else if (currentChar == 'n') return MakeNullLiteral();
        else return new Token(TokenKind.InvalidToken, '1');
    }

    private Token MakeNullLiteral()
    {  
        var currentPos = _position;
        int charCount = 4;
        string literal = _jsonString.Substring(currentPos, charCount);
        while(charCount > 1)
        {
            Next();
            charCount--;
        }

        if(literal == "null") {
            return new Token(TokenKind.NullLiteral, literal);
        }
        else return new Token(TokenKind.InvalidToken, '1');
    }

    private Token MakeBooleanLiteral()
    {
        var currentPos = _position;
        string literal = "";
        int charCount = 0;
        if (_currentChar == 't'){
            charCount = 4;
            literal = _jsonString.Substring(currentPos, charCount);
        } 
        else if (_currentChar == 'f') {
            charCount = 5;
            literal = _jsonString.Substring(currentPos, 5);
        }
        else return new Token(TokenKind.InvalidToken, '1');

        while(charCount > 1){   
            Next();
            charCount--;
        }

        if (literal == "true") return new Token(TokenKind.BooleanLiteral, true);
        else if (literal == "false") return new Token(TokenKind.BooleanLiteral, false);
        else return new Token(TokenKind.InvalidToken, '1');
    }

    private Token MakeNumericLiteral()
    {
        var currentPos = _position;
        var decimals = 0;
        while (_currentChar != '\0' && (char.IsDigit(_nextChar) || _nextChar == '.'))
        {
            if (_currentChar == '.') decimals++;
            Next();
        }

        if (decimals > 1) return new Token(TokenKind.InvalidToken, '1');
        if (decimals == 0) return new Token(TokenKind.NumericLiteral, int.Parse(_jsonString.Substring(currentPos, _position - currentPos + 1)));
        else return new Token(TokenKind.NumericLiteral, double.Parse(_jsonString.Substring(currentPos, _position - currentPos + 1)));
    }

    private Token MakeStringLiteral()
    {
        // shift to the next char..
        Next();
        var currentPos = _position;

        while (_currentChar != '\0' && _currentChar != '"')
        {
            Next();
        }

        var literal = _jsonString.Substring(currentPos, _position - currentPos);
        return new Token(TokenKind.StringLiteral, literal);
    }
}

