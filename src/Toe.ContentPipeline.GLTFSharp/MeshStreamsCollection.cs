using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using SharpGLTF.Schema2;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class MeshStreamsCollection : IEnumerable<KeyValuePair<string, IMeshStream>>
    {
        private readonly Dictionary<string, MeshStream> _accessors = new Dictionary<string, MeshStream>();

        public IEnumerator<KeyValuePair<string, IMeshStream>> GetEnumerator()
        {
            foreach (var meshStream in _accessors)
                yield return new KeyValuePair<string, IMeshStream>(meshStream.Key, meshStream.Value.Stream);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Register(string key, Accessor accessor)
        {
            if (_accessors.TryGetValue(key, out var existingStream))
            {
                if (accessor.Dimensions != existingStream.Dimensions)
                    throw new NotImplementedException(
                        $"Inconsistent dimensions for {key} stream are not supported yet ({accessor.Dimensions} != {existingStream.Dimensions})");
            }
            else
            {
                var meshStream = new MeshStream
                {
                    Dimensions = accessor.Dimensions
                };
                switch (accessor.Dimensions)
                {
                    case DimensionType.SCALAR:
                        meshStream.Stream = new ListMeshStream<float>(StreamConverterFactory.Default);
                        break;
                    case DimensionType.VEC2:
                        meshStream.Stream = new ListMeshStream<Vector2>(StreamConverterFactory.Default);
                        break;
                    case DimensionType.VEC3:
                        meshStream.Stream = new ListMeshStream<Vector3>(StreamConverterFactory.Default);
                        break;
                    case DimensionType.VEC4:
                        meshStream.Stream = new ListMeshStream<Vector4>(StreamConverterFactory.Default);
                        break;
                    default:
                        throw new NotImplementedException($"{accessor.Dimensions} not supported yet.");
                }

                _accessors.Add(key, meshStream);
            }
        }

        internal class MeshStream
        {
            public DimensionType Dimensions { get; set; }
            public IMeshStream Stream { get; set; }
        }
    }
}