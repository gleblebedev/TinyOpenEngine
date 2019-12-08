using System;
using System.Collections.Generic;
using System.Globalization;

namespace Toe.ContentPipeline.Tokenizer
{
    public class JsonParser : ITokenObserver<DefaultTokenType>
    {
        Stack<bool> _isInObject = new Stack<bool>();

        private bool IsInObject
        {
            get { return _isInObject.Peek(); }
        }
        private bool IsInArray
        {
            get { return !_isInObject.Peek(); }
        }
        public JsonParser()
        {

        }
        public delegate void Handler(Token<DefaultTokenType> token);
        private Handler _currentHandler;

        private readonly IJsonReader _reader;

        public JsonParser(IJsonReader reader)
        {
            _reader = reader;
            _currentHandler = HandleValue;
            _isInObject.Push(true);
        }

        private void HandleValue(Token<DefaultTokenType> token)
        {
            switch (token.Type)
            {
                case DefaultTokenType.Separator:
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
                        else if (token[0] == '{')
                        {
                            StartObject();
                            return;
                        }
                        else if (token[0] == '}')
                        {
                            EndObject();
                            return;
                        }
                        if (token[0] == ',')
                        {
                            _reader.OnNull();
                            NextAttributeOrEndOfObject(token);
                            return;
                        }
                    }
                    break;
                case DefaultTokenType.Int:
                    _reader.OnInteger(long.Parse(token.ToString(), CultureInfo.InvariantCulture));
                    _currentHandler = NextAttributeOrEndOfObject;
                    return;
                case DefaultTokenType.Float:
                    _reader.OnFloat(double.Parse(token.ToString(), CultureInfo.InvariantCulture));
                    _currentHandler = NextAttributeOrEndOfObject;
                    return;
                case DefaultTokenType.CharConstant:
                case DefaultTokenType.StringConstant:
                case DefaultTokenType.String:
                    _reader.OnString(token.ToString());
                    _currentHandler = NextAttributeOrEndOfObject;
                    return;
                case DefaultTokenType.Id:
                    var tokenStr = token.ToString();
                    if (0 == string.Compare(tokenStr, "true", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _reader.OnBool(true);
                    }
                    else if (0 == string.Compare(tokenStr, "false", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _reader.OnBool(false);
                    }
                    else if (0 == string.Compare(tokenStr, "null", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _reader.OnNull();
                    }
                    else
                    {
                        _reader.OnString(tokenStr);
                    }

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
            if (res == true)
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


        private void NextAttributeOrEndOfObject(Token<DefaultTokenType> token)
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
                    {
                        _currentHandler = HandleValue;
                    }
                    else
                    {
                        _currentHandler = HandleAttributeName;
                    }
                    return;
                }
            }
            throw new FormatException("Expected , or " + (IsInArray ? "]" : "}"));
        }

        private void HandleAttributeName(Token<DefaultTokenType> token)
        {
            switch (token.Type)
            {
                case DefaultTokenType.Separator:
                    if (token[0] == '}')
                    {
                        NextAttributeOrEndOfObject(token);
                        return;
                    }
                    break;
                case DefaultTokenType.CharConstant:
                case DefaultTokenType.StringConstant:
                case DefaultTokenType.String:
                case DefaultTokenType.Id:
                    _reader.OnAttribute(token.ToString());
                    _currentHandler = ExpectAssignOperator;
                    return;
            }
            throw new FormatException("Expected attribute name");
        }

        private void ExpectAssignOperator(Token<DefaultTokenType> token)
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

        public void OnNext(Token<DefaultTokenType> token)
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
    }
}