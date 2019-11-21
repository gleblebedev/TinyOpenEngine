using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class ConvertorCollection
    {
        protected Dictionary<Type, Delegate> map = new Dictionary<Type, Delegate>();

        protected StreamMetaInfo metaInfo;

        public StreamMetaInfo MetaInfo
        {
            get { return metaInfo; }
            set { metaInfo = value; }
        }

        public void CopyFrom(ConvertorCollection convertorCollection)
        {
            foreach (var d in convertorCollection.map)
            {
                map[d.Key] = d.Value;
            }
        }
    }

public class ConvertorCollection<T> : ConvertorCollection
    {
        public delegate StreamConverter<T2> ConverterFactory<T2>(IList<T> arg);

        public ConvertorCollection<T> RegisterConverter<T2>(Func<IList<T>, StreamConverter<T2>> converter)
        {
            map[typeof(T2)] = converter;
            return this;
        }

        public ConvertorCollection<T> RegisterConverter<T2>(Func<T, T2> converter)
        {
            if (typeof(T) != typeof(T2))
                map[typeof(T2)] = (ConverterFactory<T2>)((IList<T> x) => new StreamConverterImpl<T, T2>(converter, x));
            return this;
        }

        public ConverterFactory<TDst> ResolveConverter<TDst>()
        {
            Delegate v;
            if (map.TryGetValue(typeof(TDst), out v))
            {
                return (ConverterFactory<TDst>)v;
            }
            return null;
        }
    }
}