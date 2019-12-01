using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public partial class StreamConverterFactory : IStreamConverterFactory
    {
        private readonly TypeMetaInfoCollection collection = new TypeMetaInfoCollection();

        private static IEnumerable<T> EnumerateVector<T>(T x)
        {
            yield return x;
        }
        private static IEnumerable<T> EnumerateVector<T>(T x, T y)
        {
            yield return x;
            yield return y;
        }
        private static IEnumerable<T> EnumerateVector<T>(T x, T y, T z)
        {
            yield return x;
            yield return y;
            yield return z;
        }

        private static IEnumerable<T> EnumerateVector<T>(T x, T y, T z, T w)
        {
            yield return x;
            yield return y;
            yield return z;
            yield return w;
        }

        protected ConvertorCollection<T> RegisterType<T>(StreamMetaInfo streamMetaInfo)
        {
            return collection.Register<T>(streamMetaInfo);
        }

        public void Copy(StreamConverterFactory other)
        {
            collection.Copy(other.collection);
            //foreach (var reg in other.map)
            //{
            //    this.map[reg.Key] = reg.Value;
            //}
        }

        #region Implementation of IStreamConverterFactory

        public StreamConverter<TRes> ResolveConverter<T, TRes>(IList<T> arrayMeshStream)
        {
            var d = collection.ResolveConverter<T, TRes>();
            return d == null ? null : d(arrayMeshStream);
        }

        public IStreamMetaInfo GetMetaInfo(Type type)
        {
            return collection.GetMetaInfo(type);
        }

        #endregion
    }
}