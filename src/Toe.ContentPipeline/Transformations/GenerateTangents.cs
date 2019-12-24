using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline.Transformations
{
    public class GenerateTangents : IMeshTransformation
    {

        public IEnumerable<IMesh> Apply(IMesh mesh)
        {
            if (mesh.BufferViews.All(_ => _.GetStream(StreamKey.Tangent) != null))
            {
                yield return mesh;
                yield break;
            }

            var gpuMesh = mesh.ToGpuMesh();
            CalculateTangents(gpuMesh);
            yield return gpuMesh;
        }

        private void CalculateTangents(GpuMesh mesh)
        {
            foreach (var bufferAndPrimitives in mesh.GroupPrimitives())
            {
                var bufferView = bufferAndPrimitives.BufferView;
                if (bufferView.GetStream(StreamKey.TexCoord0) == null)
                {
                    continue;
                }

                throw new NotImplementedException();
            }
            //if (mesh.GetStream(StreamKey.TexCoord0) == null)
            //{
            //    return;
            //}
            //var tangents = new ArrayMeshStream<Vector3>(mesh.Count, mesh.GetStream(StreamKey.Position).ConverterFactory);
            //mesh.SetStream(StreamKey.Tangent, tangents);

            //foreach (var submesh in mesh.Submeshes)
            //{
            //    switch (submesh.VertexSourceType)
            //    {
            //        case VertexSourceType.Triangles:
            //            CalculateTangentsForTriangles(mesh, submesh, tangents);
            //            break;
            //        case VertexSourceType.Points:
            //        case VertexSourceType.Lines:
            //        case VertexSourceType.LineLoop:
            //        case VertexSourceType.LineStrip:
            //            break;
            //        default:
            //            throw new NotImplementedException(submesh.VertexSourceType.ToString());
            //    }
            //}

            //for (var index = 0; index < tangents.Count; index++)
            //{
            //    var tangent = tangents[index];
            //    var l = tangent.LengthSquared;
            //    if (l < 1e-6f)
            //    {
            //        tangents[index] = Vector3.UnitX;
            //    }
            //    else
            //    {
            //        tangents[index] = tangent * (1.0f / (float)Math.Sqrt(l));
            //    }
            //}
        }

        //private void CalculateTangentsForTriangles(SingleStreamMesh mesh, SingleStreamSubmesh submesh, ArrayMeshStream<Vector3> tangents)
        //{
        //    var positions = mesh.GetStreamReader<Vector3>(StreamKey.Position);
        //    var positionIndices = submesh.GetIndexReader(StreamKey.Position);
        //    var texCoords = mesh.GetStreamReader<Vector2>(StreamKey.TexCoord0);
        //    var texCoordIndices = submesh.GetIndexReader(StreamKey.TexCoord0);
        //    for (var index = 0; index < positionIndices.Count; index += 3)
        //    {
        //        var v1 = positions[positionIndices[index]];
        //        var v2 = positions[positionIndices[index + 1]];
        //        var v3 = positions[positionIndices[index + 2]];

        //        var w1 = texCoords[texCoordIndices[index]];
        //        var w2 = texCoords[texCoordIndices[index + 1]];
        //        var w3 = texCoords[texCoordIndices[index + 2]];

        //        var s1 = w2.X - w1.X;
        //        var s2 = w3.X - w1.X;
        //        var t1 = w2.Y - w1.Y;
        //        var t2 = w3.Y - w1.Y;

        //        var denom = s1 * t2 - s2 * t1;

        //        if (Math.Abs(denom) < float.Epsilon)
        //        {
        //            continue;
        //        }
        //        var r = 1.0f / denom;

        //        var x1 = v2.X - v1.X;
        //        var x2 = v3.X - v1.X;
        //        var y1 = v2.Y - v1.Y;
        //        var y2 = v3.Y - v1.Y;
        //        var z1 = v2.Z - v1.Z;
        //        var z2 = v3.Z - v1.Z;

        //        var tangent = new Vector3()
        //        {
        //            X = (t2 * x1 - t1 * x2) * r,
        //            Y = (t2 * y1 - t1 * y2) * r,
        //            Z = (t2 * z1 - t1 * z2) * r,
        //        };

        //        tangents[positionIndices[index]] += tangent;
        //        tangents[positionIndices[index + 1]] += tangent;
        //        tangents[positionIndices[index + 2]] += tangent;
        //    }
        //}
    }
}