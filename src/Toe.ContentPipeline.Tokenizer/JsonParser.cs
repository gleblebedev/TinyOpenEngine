using System;
using System.Collections.Generic;
using System.Globalization;

namespace Toe.ContentPipeline.Tokenizer
{
    public class JsonParser : ITokenObserver<SimpleTokenizer.TokenType>
    {
        public delegate void Handler(Token<SimpleTokenizer.TokenType> token);

        private readonly IJsonReader _reader;
        private Handler _currentHandler;
        private readonly Stack<bool> _isInObject = new Stack<bool>();

        public JsonParser()
        {
        }

        public JsonParser(IJsonReader reader)
        {
            _reader = reader;
            _currentHandler = HandleValue;
            _isInObject.Push(true);
        }

        private bool IsInObject => _isInObject.Peek();

        private bool IsInArray => !_isInObject.Peek();

        public void OnNext(Token<SimpleTokenizer.TokenType> token)
        {
            _currentHandler(token);
        }

        public void OnError(Exception exception)
        {
            _reader.OnError(exception);
        }

        public void OnCompleted()
        {
            _reader.OnCompleted();
        }

        private void HandleValue(Token<SimpleTokenizer.TokenType> token)
        {
            switch (token.Type)
            {
                case SimpleTokenizer.TokenType.Separator:
                    if (token.Length == 1)
                    {
                        if (token[0] == '[')
                        {
                            StartArray();
                            return;
                        }

                        if (token[0] == ']')
                        {
                            EndArray();
                            return;
                        }

                        if (token[0] == '{')
                        {
                            StartObject();
                            return;
                        }

                        if (token[0] == '}')
                        {
                            EndObject();
                            return;
                        }

                        if (token[0] == ',')
                        {
                            _reader.OnNull();
                            NextAttributeOrEndOfObject(token);
                        }
                    }

                    break;
                case SimpleTokenizer.TokenType.Int:
                    _reader.OnInteger(long.Parse(token.ToString(), CultureInfo.InvariantCulture));
                    _currentHandler = NextAttributeOrEndOfObject;
                    return;
                case SimpleTokenizer.TokenType.Float:
                    _reader.OnFloat(double.Parse(token.ToString(), CultureInfo.InvariantCulture));
                    _currentHandler = NextAttributeOrEndOfObject;
                    return;
                case SimpleTokenizer.TokenType.CharConstant:
                case SimpleTokenizer.TokenType.StringConstant:
                case SimpleTokenizer.TokenType.String:
                    _reader.OnString(token.ToString());
                    _currentHandler = NextAttributeOrEndOfObject;
                    return;
                case SimpleTokenizer.TokenType.Id:
                    var tokenStr = token.ToString();
                    if (0 == string.Compare(tokenStr, "true", StringComparison.CurrentCultureIgnoreCase))
                        _reader.OnBool(true);
                    else if (0 == string.Compare(tokenStr, "false", StringComparison.CurrentCultureIgnoreCase))
                        _reader.OnBool(false);
                    else if (0 == string.Compare(tokenStr, "null", StringComparison.CurrentCultureIgnoreCase))
                        _reader.OnNull();
                    else
                        _reader.OnString(tokenStr);

                    _currentHandler = NextAttributeOrEndOfObject;
                    break;
                default:
                    throw new FormatException("Expected attribute value");
            }
        }

        private void StartArray()
        {
            _isInObject.Push(false);
            _reader.OnStartArray();
            _currentHandler = HandleValue;
        }

        private void StartObject()
        {
            _isInObject.Push(true);
            _reader.OnStartObject();
            _currentHandler = HandleAttributeName;
        }

        private void EndArray()
        {
            var res = _isInObject.Pop();
            if (res)
                throw new FormatException("Expected object end but found an end of array.");
            _reader.OnEndArray();
            _currentHandler = NextAttributeOrEndOfObject;
        }

        private void EndObject()
        {
            var res = _isInObject.Pop();
            if (res == false)
                throw new FormatException("Expected array end but found an end of object.");
            _reader.OnEndObject();
            _currentHandler = NextAttributeOrEndOfObject;
        }


        private void NextAttributeOrEndOfObject(Token<SimpleTokenizer.TokenType> token)
        {
            if (token.Length == 1)
            {
                if (token[0] == '}')
                {
                    EndObject();
                    return;
                }

                if (token[0] == ']')
                {
                    EndArray();
                    return;
                }

                if (token[0] == ',')
                {
                    if (IsInArray)
                        _currentHandler = HandleValue;
                    else
                        _currentHandler = HandleAttributeName;
                    return;
                }
            }

            throw new FormatException("Expected , or " + (IsInArray ? "]" : "}"));
        }

        private void HandleAttributeName(Token<SimpleTokenizer.TokenType> token)
        {
            switch (token.Type)
            {
                case SimpleTokenizer.TokenType.Separator:
                    if (token[0] == '}')
                    {
                        NextAttributeOrEndOfObject(token);
                        return;
                    }

                    break;
                case SimpleTokenizer.TokenType.CharConstant:
                case SimpleTokenizer.TokenType.StringConstant:
                case SimpleTokenizer.TokenType.String:
                case SimpleTokenizer.TokenType.Id:
                    _reader.OnAttribute(token.ToString());
                    _currentHandler = ExpectAssignOperator;
                    return;
            }

            throw new FormatException("Expected attribute name");
        }

        private void ExpectAssignOperator(Token<SimpleTokenizer.TokenType> token)
        {
            if (token.Length == 1)
            {
                if (token[0] == ':')
                {
                    _currentHandler = HandleValue;
                    return;
                }

                if (token[0] == ',')
                {
                    _reader.OnAttributeNull();
                    _currentHandler = HandleAttributeName;
                    return;
                }
            }

            throw new FormatException("Expected :");
        }
    }
}