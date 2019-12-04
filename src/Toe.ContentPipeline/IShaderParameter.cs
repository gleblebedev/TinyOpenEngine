using System;

namespace Toe.ContentPipeline
{
    public interface IShaderParameter<T> : IShaderParameter
    {
        T Value { get; }
    }

    public interface IShaderParameter
    {
        string Key { get; }

        Type ValueType { get; }
    }
}