using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public interface IJsonReader
    {
        void OnNull();
        void OnBool(bool value);
        void OnInteger(long value);
        void OnFloat(double value);
        void OnString(string value);
        void OnAttributeNull();
        void OnStartArray();
        void OnStartObject();
        void OnEndArray();
        void OnEndObject();
        void OnAttribute(string attributeName);
        void OnError(Exception exception);
        void OnCompleted();
    }
}