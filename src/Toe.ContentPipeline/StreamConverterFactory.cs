using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public class StreamConverterFactory : IStreamConverterFactory
    {
        public static readonly StreamConverterFactory Default;

        private readonly TypeMetaInfoCollection collection = new TypeMetaInfoCollection();

        static StreamConverterFactory()
        {
            Default = new StreamConverterFactory();
            Default
                .RegisterType<Vector3>(new StreamMetaInfo(typeof(float), typeof(Vector3), 3, 1))
                .RegisterConverter(c => new Vector4(c.X, c.Y, c.Z, 0))
                .RegisterConverter(c => new Vector3(c.X, c.Y, c.Z))
                .RegisterConverter(c => new Vector2(c.X, c.Y))
                .RegisterConverter(c => c.X)
                .RegisterConverter(c => new Vector4d(c.X, c.Y, c.Z, 0))
                .RegisterConverter(c => new Vector3d(c.X, c.Y, c.Z))
                .RegisterConverter(c => new Vector2d(c.X, c.Y))
                .RegisterConverter(c => new Vector4i((int) c.X, (int) c.Y, (int) c.Z, 0))
                .RegisterConverter(c => new Vector3i((int) c.X, (int) c.Y, (int) c.Z))
                .RegisterConverter(c => new Vector2i((int) c.X, (int) c.Y))
                .RegisterConverter(c => new Vector4l((long) c.X, (long) c.Y, (long) c.Z, 0))
                .RegisterConverter(c => new Vector3l((long) c.X, (long) c.Y, (long) c.Z))
                .RegisterConverter(c => new Vector2l((long) c.X, (long) c.Y))
                .RegisterConverter(c => EnumerateVector(c));

            //Default
            //    .RegisterType<Vector3i>(new StreamMetaInfo(typeof(int), typeof(Vector3i), 3, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)c.Z, 0))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)c.Z))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, 0))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, 0))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, 0))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));

            //Default
            //    .RegisterType<Vector3i>(new StreamMetaInfo(typeof(long), typeof(Vector3l), 3, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)c.Z, 0))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)c.Z))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, 0))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, 0))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, 0))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));

            //Default
            //    .RegisterType<Vector3d>(new StreamMetaInfo(typeof(double), typeof(Vector3d), 3, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)c.Z, 0))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)c.Z))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, 0))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, 0))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, 0))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));

            Default
                .RegisterType<Vector4>(new StreamMetaInfo(typeof(float), typeof(Vector4), 4, 1))
                .RegisterConverter(c => new Vector4(c.X, c.Y, c.Z, c.W))
                .RegisterConverter(c => new Vector3(c.X, c.Y, c.Z))
                .RegisterConverter(c => new Vector2(c.X, c.Y))
                .RegisterConverter(c => c.X)
                //.RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, (double)c.W))
                //.RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
                //.RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
                //.RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, (int)c.W))
                //.RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
                //.RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
                //.RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, (long)c.W))
                //.RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
                //.RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
                .RegisterConverter(c => EnumerateVector(c));
            //Default
            //    .RegisterType<Vector4i>(new StreamMetaInfo(typeof(int), typeof(Vector4i), 4, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)c.Z, (float)c.W))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)c.Z))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, (double)c.W))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, (int)c.W))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, (long)c.W))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));
            //Default
            //    .RegisterType<Vector4l>(new StreamMetaInfo(typeof(long), typeof(Vector4l), 4, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)c.Z, (float)c.W))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)c.Z))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, (double)c.W))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, (int)c.W))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, (long)c.W))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));
            //Default
            //    .RegisterType<Vector4d>(new StreamMetaInfo(typeof(double), typeof(Vector4d), 4, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)c.Z, (float)c.W))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)c.Z))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)c.Z, (double)c.W))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)c.Z))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)c.Z, (int)c.W))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)c.Z))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)c.Z, (long)c.W))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)c.Z))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));

            Default
                .RegisterType<Vector2>(new StreamMetaInfo(typeof(float), typeof(Vector2), 2, 1))
                .RegisterConverter(c => new Vector4(c.X, c.Y, 0, 0))
                .RegisterConverter(c => new Vector3(c.X, c.Y, 0))
                .RegisterConverter(c => new Vector2(c.X, c.Y))
                .RegisterConverter(c => c.X)
                //.RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)0, 0))
                //.RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)0))
                //.RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
                //.RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)0, 0))
                //.RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)0))
                //.RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
                //.RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)0, 0))
                //.RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)0))
                //.RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
                .RegisterConverter(c => EnumerateVector(c));
            //Default
            //    .RegisterType<Vector2i>(new StreamMetaInfo(typeof(int), typeof(Vector2i), 2, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)0, 0))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)0))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)0, 0))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)0))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)0, 0))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)0))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)0, 0))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)0))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));
            //Default
            //    .RegisterType<Vector2l>(new StreamMetaInfo(typeof(long), typeof(Vector2l), 2, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)0, 0))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)0))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)0, 0))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)0))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)0, 0))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)0))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)0, 0))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)0))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));
            //Default
            //    .RegisterType<Vector2d>(new StreamMetaInfo(typeof(double), typeof(Vector2d), 2, 1))
            //    .RegisterConverter(c => new Vector4((float)c.X, (float)c.Y, (float)0, 0))
            //    .RegisterConverter(c => new Vector3((float)c.X, (float)c.Y, (float)0))
            //    .RegisterConverter(c => new Vector2((float)c.X, (float)c.Y))
            //    .RegisterConverter(c => new Vector1((float)c.X))
            //    .RegisterConverter(c => new Vector4d((double)c.X, (double)c.Y, (double)0, 0))
            //    .RegisterConverter(c => new Vector3d((double)c.X, (double)c.Y, (double)0))
            //    .RegisterConverter(c => new Vector2d((double)c.X, (double)c.Y))
            //    .RegisterConverter(c => new Vector4i((int)c.X, (int)c.Y, (int)0, 0))
            //    .RegisterConverter(c => new Vector3i((int)c.X, (int)c.Y, (int)0))
            //    .RegisterConverter(c => new Vector2i((int)c.X, (int)c.Y))
            //    .RegisterConverter(c => new Vector4l((long)c.X, (long)c.Y, (long)0, 0))
            //    .RegisterConverter(c => new Vector3l((long)c.X, (long)c.Y, (long)0))
            //    .RegisterConverter(c => new Vector2l((long)c.X, (long)c.Y))
            //    .RegisterConverter(c => EnumerateVector(c));

            Default
                .RegisterType<float>(new StreamMetaInfo(typeof(float), typeof(float), 1, 1))
                .RegisterConverter(c => new Vector4(c, 0, 0, 0))
                .RegisterConverter(c => new Vector3(c, 0, 0))
                .RegisterConverter(c => new Vector2(c, 0))
                .RegisterConverter(c => EnumerateVector(c));
        }

        private static IEnumerable<float> EnumerateVector(float vec)
        {
            yield return vec;
        }

        private static IEnumerable<float> EnumerateVector(Vector2 vec)
        {
            yield return vec.X;
            yield return vec.Y;
        }

        //private static IEnumerable<float> EnumerateVector(Vector2i vec)
        //{
        //    yield return vec.X;
        //    yield return vec.Y;
        //}
        //private static IEnumerable<float> EnumerateVector(Vector2l vec)
        //{
        //    yield return vec.X;
        //    yield return vec.Y;
        //}
        //private static IEnumerable<float> EnumerateVector(Vector2d vec)
        //{
        //    yield return (float)vec.X;
        //    yield return (float)vec.Y;
        //}
        private static IEnumerable<float> EnumerateVector(Vector3 vec)
        {
            yield return vec.X;
            yield return vec.Y;
            yield return vec.Z;
        }

        //private static IEnumerable<float> EnumerateVector(Vector3i vec)
        //{
        //    yield return vec.X;
        //    yield return vec.Y;
        //    yield return vec.Z;
        //}
        //private static IEnumerable<float> EnumerateVector(Vector3l vec)
        //{
        //    yield return vec.X;
        //    yield return vec.Y;
        //    yield return vec.Z;
        //}
        //private static IEnumerable<float> EnumerateVector(Vector3d vec)
        //{
        //    yield return (float)vec.X;
        //    yield return (float)vec.Y;
        //    yield return (float)vec.Z;
        //}
        private static IEnumerable<float> EnumerateVector(Vector4 vec)
        {
            yield return vec.X;
            yield return vec.Y;
            yield return vec.Z;
            yield return vec.W;
        }

        //private static IEnumerable<float> EnumerateVector(Vector4i vec)
        //{
        //    yield return vec.X;
        //    yield return vec.Y;
        //    yield return vec.Z;
        //    yield return vec.W;
        //}
        //private static IEnumerable<float> EnumerateVector(Vector4l vec)
        //{
        //    yield return vec.X;
        //    yield return vec.Y;
        //    yield return vec.Z;
        //    yield return vec.W;
        //}
        //private static IEnumerable<float> EnumerateVector(Vector4d vec)
        //{
        //    yield return (float)vec.X;
        //    yield return (float)vec.Y;
        //    yield return (float)vec.Z;
        //    yield return (float)vec.W;
        //}
        protected ConvertorCollection<T> RegisterType<T>(StreamMetaInfo streamMetaInfo)
        {
            return collection.Register<T>(streamMetaInfo);
        }

        public void Copy(StreamConverterFactory other)
        {
            collection.Copy(other.collection);
            //foreach (var reg in other.map)
            //{
            //	this.map[reg.Key] = reg.Value;
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