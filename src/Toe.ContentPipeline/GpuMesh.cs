using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public class GpuMesh : AbstractMesh<GpuPrimitive>, IMesh
    {
        public GpuMesh(string id) : base(id)
        {
        }

        public GpuMesh()
        {
        }

        public List<GpuPrimitive> Primitives => _primitives;
        IList<IMeshPrimitive> IMesh.Primitives => _abstractPrimitives;

        public static GpuMesh Optimize(IMesh source)
        {
            var optimalIndexedMesh = IndexedMesh.Optimize(source);
            var resultPrimitives = new GpuPrimitive[optimalIndexedMesh.Primitives.Count];
            var result = new GpuMesh(source.Id);

            var indices = new List<int>();
            foreach (var primitiveGroup in optimalIndexedMesh.GroupPrimitives())
            {
                var sourceBuffer = primitiveGroup.BufferView;
                var gpuBufferView = new MeshBufferView();
                var streamKeys = sourceBuffer.GetStreams();
                var copiers = streamKeys.Project((streamKey, streamIndex) =>
                {
                    var meshStream = sourceBuffer.GetStream(streamKey);
                    var destination = meshStream.CreateListMeshStreamOfTheSameType();
                    gpuBufferView.SetStream(streamKey, destination);
                    return new BoxingtMeshStreamCopier(meshStream, destination);
                });

                var expectedCapacityEsitmation =
                    primitiveGroup.Select(_ => _.GetIndexReader(StreamKey.Position).Count).Sum() * streamKeys.Count;
                indices.Clear();
                if (indices.Capacity < expectedCapacityEsitmation) indices.Capacity = expectedCapacityEsitmation;
                var vertexMap = new Dictionary<IndexSet, int>();
                foreach (var primitiveAndIndex in primitiveGroup.Primitives)
                {
                    var primitive = primitiveAndIndex.Primitive;
                    var setReader = new IndexSetReader(indices, streamKeys, primitive);
                    var ints = primitive.GetIndexReader(StreamKey.Position);
                    var gpuIndices = new List<int>();
                    for (var index = 0; index < ints.Count; index++)
                    {
                        var set = setReader.Read(index);
                        if (vertexMap.TryGetValue(set, out var vertexIndex))
                        {
                            setReader.Position = set.Offset;
                        }
                        else
                        {
                            for (var streamIndex = 0; streamIndex < streamKeys.Count; streamIndex++)
                                vertexIndex = copiers[streamIndex].Copy(set[streamIndex]);
                            vertexMap.Add(set, vertexIndex);
                        }

                        gpuIndices.Add(vertexIndex);
                    }

                    var gpuPrimitive = new GpuPrimitive(primitive.Topology, gpuIndices, gpuBufferView);
                    resultPrimitives[primitiveAndIndex.Index] = gpuPrimitive;
                }
            }

            foreach (var indexMeshPrimitive in resultPrimitives) result.Primitives.Add(indexMeshPrimitive);

            return result;
        }
    }
}