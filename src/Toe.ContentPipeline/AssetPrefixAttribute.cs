using System;
using System.Reflection;

namespace Toe.ContentPipeline
{
    public class AssetPrefixAttribute:Attribute
    {
        private readonly string _prefix;

        public AssetPrefixAttribute(string prefix)
        {
            _prefix = prefix;
        }

        public string Prefix
        {
            get => _prefix;
        }

        public static string Get(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(AssetPrefixAttribute), true);
            if (attributes == null || attributes.Length < 0)
                return type.Name;
            return ((AssetPrefixAttribute) attributes[0]).Prefix;
        }
    }
}