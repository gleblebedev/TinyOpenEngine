using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public class IndexedMesh : AbstractMesh<IndexMeshPrimitive>, IMesh
    {
        public IndexedMesh(string id) : base(id)
        {
        }

        public IndexedMesh() : base()
        {
        }

        public IList<IndexMeshPrimitive> Primitives => _primitives;
        IList<IMeshPrimitive> IMesh.Primitives => _abstractPrimitives;

        public static IndexedMesh Optimize(IMesh mesh)
        {
            var result = new IndexedMesh(mesh.Id);
            var resultPrimitives = new IndexMeshPrimitive[mesh.Primitives.Count];
            var primitiveGroups = mesh.GroupPrimitives();

            foreach (var primitiveGroup in primitiveGroups)
            {
                var resultBufferView = new MeshBufferView();
       
                var bufferView = primitiveGroup.BufferView;
                var streamKeys = bufferView.GetStreams().ToList();
                var sourceStreams = streamKeys.Select(x => bufferView.GetStream(x)).ToList();
                var dictionaryStreams = sourceStreams.Select(_ => _.CreateDictionaryMeshStreamOfTheSameType()).ToList();
                for (var index = 0; index < dictionaryStreams.Count; index++)
                {
                    resultBufferView.SetStream(streamKeys[index], dictionaryStreams[index]);
                }

                foreach (var primitiveAndIndex in primitiveGroup.Primitives)
                {
                    var meshPrimitive = new IndexMeshPrimitive(resultBufferView);
                    resultPrimitives[primitiveAndIndex.Index] = meshPrimitive;
                    var submesh = primitiveAndIndex.Primitive;
                    meshPrimitive.Topology = submesh.Topology;

                    for (var keyIndex = 0; keyIndex < streamKeys.Count; ++keyIndex)
                    {
                        var key = streamKeys[keyIndex];
                        var sourceIndices = submesh.GetIndexReader(key);
                        var dataStream = dictionaryStreams[keyIndex];
                        var sourceStream = sourceStreams[keyIndex];
                        var stream = new List<int>(sourceIndices.Count);
                        for (var i = 0; i < sourceIndices.Count; ++i)
                        {
                            stream.Add(dataStream.Add(sourceStream[sourceIndices[i]]));
                        }
                        meshPrimitive.SetIndexStream(key, stream);
                    }
                }
                for (var keyIndex = 0; keyIndex < streamKeys.Count; ++keyIndex)
                {
                    bufferView.SetStream(streamKeys[keyIndex], dictionaryStreams[keyIndex]);
                }
            }

            foreach (var indexMeshPrimitive in resultPrimitives)
            {
                result.Primitives.Add(indexMeshPrimitive);
            }
            return result;
        }
    }
}