using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Toe.ContentPipeline.Transformations
{
    public class GenerateNormals : IMeshTransformation
    {
        public IEnumerable<IMesh> Apply(IMesh mesh)
        {
            if (mesh.BufferViews.All(_ => _.GetStream(StreamKey.Normal) != null))
            {
                yield return mesh;
                yield break;
            }

            IndexedMesh indexedMesh;
            indexedMesh = mesh as IndexedMesh ?? mesh.ToIndexedMesh();
            EvaluateNormals(indexedMesh);
            yield return indexedMesh;
        }

        private void EvaluateNormals(IndexedMesh indexedMesh)
        {
            foreach (var bufferAndPrimitives in indexedMesh.GroupPrimitives())
            {
                var bufferView = bufferAndPrimitives.BufferView;
                var existingNormals = bufferView.GetStream(StreamKey.Normal);
                if (existingNormals != null)
                    continue;

                var positions = bufferView.GetStreamReader<Vector3>(StreamKey.Position);
                var normals = new ArrayMeshStream<Vector3>(positions.Count, StreamConverterFactory.Default);
                foreach (var primitive in bufferAndPrimitives)
                    primitive.SetIndexStream(StreamKey.Normal, primitive.GetIndexReader(StreamKey.Position).ToList());
                {
                    foreach (var face in indexedMesh.Primitives.SelectMany(_ => _.GetFaces(StreamKey.Position)))
                    {
                        var a = positions[face.Item1];
                        var b = positions[face.Item2];
                        var c = positions[face.Item3];
                        var n = Vector3.Cross(b - a, c - a);
                        normals[face.Item1] += n;
                        normals[face.Item2] += n;
                        normals[face.Item3] += n;
                    }

                    for (var index = 0; index < normals.Count; index++)
                    {
                        var normal = normals[index];
                        if (normal != Vector3.Zero)
                            normals[index] = Vector3.Normalize(normal);
                    }

                    bufferView.SetStream(StreamKey.Normal, normals);
                }
                foreach (var primitive in bufferAndPrimitives)
                    primitive.SetIndexStream(StreamKey.Normal, primitive.GetIndexReader(StreamKey.Position).ToList());
            }
        }
    }
}