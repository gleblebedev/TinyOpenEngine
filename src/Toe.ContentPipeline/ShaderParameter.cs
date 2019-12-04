using System;

namespace Toe.ContentPipeline
{
    public abstract class ShaderParameter : IShaderParameter
    {
        public ShaderParameter(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public abstract Type ValueType { get; }

        public override string ToString()
        {
            return $"{ValueType.Name} {Key}";
        }

        public static ShaderParameter<T> Create<T>(string key, T value)
        {
            return new ShaderParameter<T>(key, value);
        }
    }

    public class ShaderParameter<T> : ShaderParameter, IShaderParameter<T>
    {
        public ShaderParameter(string key) : base(key)
        {
        }

        public ShaderParameter(string key, T value) : base(key)
        {
            Value = value;
        }

        public override Type ValueType => typeof(T);

        public T Value { get; set; }

        public override string ToString()
        {
            return $"{ValueType.Name} {Key} = {Value}";
        }
    }
}