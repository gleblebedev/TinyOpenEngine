using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX.Direct3D11;
using Veldrid;
using BlendStateDescription = Veldrid.BlendStateDescription;
using BufferDescription = Veldrid.BufferDescription;
using DepthStencilStateDescription = Veldrid.DepthStencilStateDescription;
using RasterizerStateDescription = Veldrid.RasterizerStateDescription;

namespace Toe.ContentPipeline.VeldridMesh
{
    public class VeldridGeometry
    {
        public Memory<byte> VertexBuffer;
        public Memory<byte> IndexBuffer;
        public IndexFormat IndexFormat;
        public VeldridPrimitive[] Primitives;

        /// <summary>
        /// Creates a new <see cref="DeviceBuffer"/> from VertexBuffer data.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device, capable of creating device resources and executing commands.</param>
        /// <param name="resourceFactory">A device object responsible for the creation of graphics resources.</param>
        /// <returns>A new <see cref="DeviceBuffer"/>.</returns>
        public DeviceBuffer CreateDeviceVertexBuffer(GraphicsDevice graphicsDevice, ResourceFactory resourceFactory = null)
        {
            return CreateDeviceBuffer(graphicsDevice, resourceFactory, ref VertexBuffer, BufferUsage.VertexBuffer);
        }

        /// <summary>
        /// Creates a new <see cref="DeviceBuffer"/> from IndexBuffer data.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device, capable of creating device resources and executing commands.</param>
        /// <param name="resourceFactory">A device object responsible for the creation of graphics resources.</param>
        /// <returns>A new <see cref="DeviceBuffer"/>.</returns>
        public DeviceBuffer CreateDeviceIndexBuffer(GraphicsDevice graphicsDevice, ResourceFactory resourceFactory = null)
        {
            return CreateDeviceBuffer(graphicsDevice, resourceFactory, ref IndexBuffer, BufferUsage.IndexBuffer);
        }

        /// <summary>
        /// Constructs a new <see cref="GraphicsPipelineDescription"/>.
        /// </summary>
        /// <param name="blendState">A description of the blend state, which controls how color values are blended into each
        /// color target.</param>
        /// <param name="depthStencilStateDescription">A description of the depth stencil state, which controls depth tests,
        /// writing, and comparisons.</param>
        /// <param name="rasterizerState">A description of the rasterizer state, which controls culling, clipping, scissor, and
        /// polygon-fill behavior.</param>
        /// <param name="shaders">An array of <see cref="Shader"/> objects, one for each shader stage which is to be active
        /// in the <see cref="Pipeline"/>. At a minimum, every graphics Pipeline must include a Vertex and Fragment shader. All
        /// other stages are optional, but if either Tessellation stage is present, then the other must also be.</param>
        /// <param name="specializations">An array of <see cref="SpecializationConstant"/> used to override specialization
        /// constants in the created <see cref="Pipeline"/>. Each element in this array describes a single ID-value pair, which
        /// will be matched with the constants specified in each <see cref="Shader"/>.</param>
        /// <param name="resourceLayouts">An array of <see cref="ResourceLayout"/>, which controls the layout of shader resoruces
        /// in the <see cref="Pipeline"/>.</param>
        /// <param name="outputs">A description of the output attachments used by the <see cref="Pipeline"/>.</param>
        public GraphicsPipelineDescription CreateGraphicsPipelineDescription(
            ref VeldridPrimitive primitive,
            BlendStateDescription blendState,
            DepthStencilStateDescription depthStencilStateDescription,
            RasterizerStateDescription rasterizerState,
            Shader[] shaders,
            SpecializationConstant[] specializations,
            ResourceLayout[] resourceLayouts,
            OutputDescription outputs)
        {
            var shaderSetDescription = new ShaderSetDescription(new VertexLayoutDescription[] { primitive.VertexLayout }, shaders, specializations);
            return new GraphicsPipelineDescription(blendState, depthStencilStateDescription, rasterizerState, primitive.PrimitiveTopology, shaderSetDescription, resourceLayouts, outputs);
        }

        /// <summary>
        /// Creates a new <see cref="DeviceBuffer"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device, capable of creating device resources and executing commands.</param>
        /// <param name="resourceFactory">A device object responsible for the creation of graphics resources.</param>
        /// <returns>A new <see cref="DeviceBuffer"/>.</returns>
        private static unsafe DeviceBuffer CreateDeviceBuffer(GraphicsDevice graphicsDevice, ResourceFactory resourceFactory,
            ref Memory<byte> memory, BufferUsage bufferUsage)
        {
            resourceFactory = resourceFactory ?? graphicsDevice.ResourceFactory;
            var deviceBuffer = resourceFactory.CreateBuffer(new BufferDescription((uint) memory.Length, bufferUsage));
            try
            {
                fixed (void* ptr = &memory.Span.GetPinnableReference())
                {
                    graphicsDevice.UpdateBuffer(null, 0, new IntPtr(ptr), (uint) memory.Length);
                }
            }
            catch
            {
                deviceBuffer.Dispose();
                throw;
            }

            return deviceBuffer;
        }

        public static VeldridGeometry Create(IMesh mesh)
        {
            var gpuMesh = mesh.ToGpuMesh();
            var result = new VeldridGeometry();
            var bufferViewToPrimitive = new Dictionary<IBufferView, VeldridPrimitive>();
            var vertexBufferMemoryStream = new MemoryStream();
            result.IndexFormat = Veldrid.IndexFormat.UInt16;
            using (var vertexBufferWriter = new BinaryWriter(vertexBufferMemoryStream))
            {
                foreach (var bufferView in gpuMesh.BufferViews)
                {
                    var veldridPrimitive = new VeldridPrimitive();
                    veldridPrimitive.VertexBufferOffset = (uint) vertexBufferMemoryStream.Position;

                    var streamKeys = bufferView.GetStreams().Select(_ => new SortableStreamKey(_)).OrderBy(_ => _)
                        .Select(_ => _.Key).ToList();
                    var elements = new VertexElementDescription[streamKeys.Count];
                    var accessors = new IStreamAccessor[streamKeys.Count];
                    for (var index = 0; index < streamKeys.Count; index++)
                    {
                        var reader = accessors[index] = GetAccessor(bufferView, streamKeys[index]);
                        elements[index] = new VertexElementDescription(streamKeys[index].ToString(),
                            VertexElementSemantic.TextureCoordinate, reader.VertexElementFormat);
                    }

                    var count = accessors[0].Count;
                    if (count > ushort.MaxValue)
                    {
                        result.IndexFormat = Veldrid.IndexFormat.UInt32;
                    }

                    for (var index = 0; index < count; index++)
                    {
                        for (var accessorIndex = 0; accessorIndex < accessors.Length; accessorIndex++)
                        {
                            accessors[accessorIndex].Write(index, vertexBufferWriter);
                        }
                    }

                    veldridPrimitive.VertexLayout = new VertexLayoutDescription(elements);
                    bufferViewToPrimitive.Add(bufferView, veldridPrimitive);
                }
            }
            result.VertexBuffer = new Memory<byte>(vertexBufferMemoryStream.ToArray());
            var indexBufferMemoryStream = new MemoryStream();
            result.Primitives = new VeldridPrimitive[gpuMesh.Primitives.Count];
            using (var indexBufferWriter = new BinaryWriter(indexBufferMemoryStream))
            {
                for (var primitiveIndex = 0; primitiveIndex < gpuMesh.Primitives.Count; primitiveIndex++)
                {
                    var primitive = gpuMesh.Primitives[primitiveIndex];
                    var primitiveBase = bufferViewToPrimitive[primitive.BufferView];
                    var veldridPrimitive = new VeldridPrimitive()
                    {
                        VertexBufferOffset = primitiveBase.VertexBufferOffset,
                        PrimitiveTopology = GetTopology(primitive.Topology),
                        IndexBufferOffset =  (uint)indexBufferMemoryStream.Position,
                        VertexLayout = primitiveBase.VertexLayout
                    };
                    if (result.IndexFormat == Veldrid.IndexFormat.UInt32)
                    {
                        foreach (var index in primitive)
                        {
                            indexBufferWriter.Write((uint)index);
                        }
                    }
                    else if (result.IndexFormat == Veldrid.IndexFormat.UInt16)
                    {
                        foreach (var index in primitive)
                        {
                            indexBufferWriter.Write((ushort)index);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    result.Primitives[primitiveIndex] = veldridPrimitive;
                }
            }

            result.IndexBuffer = new Memory<byte>(indexBufferMemoryStream.ToArray());

            return result;
        }

        private static Veldrid.PrimitiveTopology GetTopology(PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.TriangleList: return Veldrid.PrimitiveTopology.TriangleList;
                case PrimitiveTopology.TriangleStrip: return Veldrid.PrimitiveTopology.TriangleStrip;
                case PrimitiveTopology.LineList: return Veldrid.PrimitiveTopology.LineList;
                case PrimitiveTopology.LineStrip: return Veldrid.PrimitiveTopology.LineStrip;
                case PrimitiveTopology.PointList: return Veldrid.PrimitiveTopology.PointList;
                default: throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, null);
            }
        }

        private static IStreamAccessor GetAccessor(IBufferView bufferView, StreamKey streamKey)
        {
            var stream = bufferView.GetStream(streamKey);
            if (stream.MetaInfo.NumberOfSets == 1)
            {
                switch (stream.MetaInfo.ComponentsPerSet)
                {
                    case 1:
                        return new Float1StreamAccessor(streamKey, stream);
                    case 2:
                        return new Float2StreamAccessor(streamKey, stream);
                    case 3:
                        return new Float3StreamAccessor(streamKey, stream);
                    case 4:
                        return new Float4StreamAccessor(streamKey, stream);
                }
            }

            throw new NotImplementedException(streamKey+" format is not supported yet");
        }
    }
}