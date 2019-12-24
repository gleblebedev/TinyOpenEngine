using System;
using System.Collections.Generic;

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

            foreach (var primitiveGroup in optimalIndexedMesh.GroupPrimitives())
            {
                var sourceBuffer = primitiveGroup.BufferView;
            }
            foreach (var indexMeshPrimitive in resultPrimitives)
            {
                result.Primitives.Add(indexMeshPrimitive);
            }

            throw new NotImplementedException();
            return result;

            //var streamKeys = VertexBufferFormatItem.ExtractItems(source).ToArray();
            //var lastItem = streamKeys.LastOrDefault();

            //if (lastItem == null)
            //{
            //    return new SingleStreamMesh();
            //}

            //var streamMetaInfos = streamKeys.Select(x => source.GetStream(x.Key)).ToArray();
            //var streamReaders = streamKeys.Select(x => source.GetStreamReader<Vector4>(x.Key)).ToArray();

            //var totalComponents = lastItem.Components + lastItem.Offset;

            //var key = new float[totalComponents];
            //var dstStreams = new List<IMeshStream>();
            //for (var i = 0; i < streamKeys.Length; ++i)
            //{
            //    var meshStream = ListMeshStream.Create(
            //        streamMetaInfos[i].MetaInfo.BaseType,
            //        streamMetaInfos[i].ConverterFactory,
            //        streamMetaInfos[i].MetaInfo);
            //    dstStreams.Add(meshStream);
            //    dst.SetStream(streamKeys[i].Key, meshStream);
            //}

            //var map = new Dictionary<float[], int>(new FloatArrayComparer());

            //foreach (var submesh in source.Submeshes)
            //{
            //    var streamIndexReaders = streamKeys.Select(x => submesh.GetIndexReader(x.Key)).ToArray();
            //    var stream = new List<int>(submesh.Count);
            //    for (var i = 0; i < submesh.Count; ++i)
            //    {
            //        for (var j = 0; j < streamKeys.Length; j++)
            //        {
            //            var streamKey = streamKeys[j];
            //            var streamIndexReader = streamIndexReaders[j][i];
            //            var f = streamReaders[j][streamIndexReader];
            //            if (streamKey.Components >= 1)
            //            {
            //                key[streamKey.Offset] = f.X;
            //            }
            //            if (streamKey.Components >= 2)
            //            {
            //                key[streamKey.Offset + 1] = f.Y;
            //            }
            //            if (streamKey.Components >= 3)
            //            {
            //                key[streamKey.Offset + 2] = f.Z;
            //            }
            //            if (streamKey.Components >= 4)
            //            {
            //                key[streamKey.Offset + 3] = f.W;
            //            }
            //        }

            //        int index;
            //        if (!map.TryGetValue(key, out index))
            //        {
            //            var copy = new float[key.Length];
            //            key.CopyTo(copy, 0);
            //            index = map.Count;
            //            map.Add(copy, index);

            //            for (var j = 0; j < streamKeys.Length; ++j)
            //            {
            //                var sourceStream = source.GetStream(streamKeys[j].Key);
            //                var dstStream = dstStreams[j];

            //                var streamIndexReader = streamIndexReaders[j];
            //                var value = sourceStream[streamIndexReader[i]];
            //                dstStream.Add(value);
            //            }
            //        }
            //        stream.Add(index);
            //    }
            //    var dstSubMesh = dst.CreateSubmesh(stream, submesh.VertexSourceType);
            //    dstSubMesh.Material = submesh.Material;
            //}

            //return dst;
        }
    }
}