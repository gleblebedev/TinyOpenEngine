using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class TypeMetaInfoCollection
    {
        private readonly Dictionary<Type, ConvertorCollection> map = new Dictionary<Type, ConvertorCollection>();

        public ConvertorCollection<T> Register<T>(StreamMetaInfo streamMetaInfo)
        {
            var key = typeof(T);
            ConvertorCollection v;
            if (!map.TryGetValue(key, out v))
            {
                v = new ConvertorCollection<T> {MetaInfo = streamMetaInfo};
                map.Add(key, v);
            }

            v.MetaInfo = streamMetaInfo;
            return (ConvertorCollection<T>) v;
        }

        public ConvertorCollection<T>.ConverterFactory<TRes> ResolveConverter<T, TRes>()
        {
            ConvertorCollection v;
            if (map.TryGetValue(typeof(T), out v)) return ((ConvertorCollection<T>) v).ResolveConverter<TRes>();
            return null;
        }

        public IStreamMetaInfo GetMetaInfo(Type key)
        {
            ConvertorCollection v;
            if (map.TryGetValue(key, out v)) return v.MetaInfo;
            return null;
        }

        public void Copy(TypeMetaInfoCollection collection)
        {
            foreach (var convertorCollection in collection.map)
            {
                ConvertorCollection v;
                if (!map.TryGetValue(convertorCollection.Key, out v))
                {
                    v =
                        (ConvertorCollection)
                        Activator.CreateInstance(
                            typeof(ConvertorCollection<>).MakeGenericType(convertorCollection.Key));
                    v.MetaInfo = convertorCollection.Value.MetaInfo;
                    map.Add(convertorCollection.Key, v);
                }

                v.CopyFrom(convertorCollection.Value);
            }
        }
    }
}