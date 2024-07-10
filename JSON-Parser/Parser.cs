using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSON_Parser
{
    public class Parser
    {
        public Lexer _lexer { get; set; }
        private Token _currentToken { get; set; }
        private Token _nextToken { get; set; }

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = new Token(TokenKind.EOF, '\0');
            _nextToken = new Token(TokenKind.EOF, '\0');

            Next();
        }

        private void Next()
        {
            _currentToken = _nextToken;
            _nextToken = _lexer.Lex();
        }

        public object Parse()
        {
            Next();
            if (_currentToken.kind == TokenKind.InvalidToken) return _currentToken.literal;

            return _currentToken.kind switch
            {
                TokenKind.StringLiteral or TokenKind.NumericLiteral or TokenKind.BooleanLiteral or TokenKind.NullLiteral or TokenKind.CloseBracket or TokenKind.CloseCurlyBrace => _currentToken.literal,
                TokenKind.OpenCurlyBrace => MakeDictionary(),
                TokenKind.OpenBracket => MakeList(),
                _ => (object)string.Empty,
            };
        }

        private object MakeList()
        {
            var outputList = new List<object>();

            while (_currentToken.kind != TokenKind.CloseBracket && _nextToken.kind != TokenKind.CloseBracket && _currentToken.kind != TokenKind.EOF)
            {
                if (_currentToken.kind == TokenKind.InvalidToken) return '1';
                outputList.Add(Parse());
                Next();
            }

            return outputList;
        }

        private object MakeDictionary()
        {
            var outputDict = new Dictionary<object, object>();

            if (_currentToken.kind == TokenKind.OpenCurlyBrace && _nextToken.kind == TokenKind.CloseCurlyBrace)
            {
                Next();
                return outputDict;
            }

            while (_currentToken.kind != TokenKind.CloseCurlyBrace)
            {

                if (_currentToken.kind == TokenKind.InvalidToken) return '1';

                var key = Parse();
                if(_nextToken.kind == TokenKind.EOF) return outputDict;
                if (_nextToken.kind != TokenKind.Colon) return '1';

                Next();
                var value = Parse();
                outputDict.Add(key, value);
                Next();
            }

            return outputDict;
        }
    }
}