using System;

namespace Toe.ContentPipeline
{
    public class AssetPrefixAttribute : Attribute
    {
        public AssetPrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; }

        public static string Get(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(AssetPrefixAttribute), true);
            if (attributes == null || attributes.Length < 0)
                return type.Name;
            return ((AssetPrefixAttribute) attributes[0]).Prefix;
        }
    }
}