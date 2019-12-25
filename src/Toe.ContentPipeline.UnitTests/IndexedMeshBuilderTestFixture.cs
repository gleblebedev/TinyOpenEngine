﻿using System.Numerics;
using NUnit.Framework;

namespace Toe.ContentPipeline
{
    [TestFixture]
    public class IndexedMeshBuilderTestFixture
    {
        [Test]
        public void BuildQuad()
        {
            var builder = new IndexedMeshBuilder();
            builder.BeginBuffer();
            builder.BeginPrimitive(PrimitiveTopology.TriangleList);
            builder.Position(new Vector3(0, 0, 0));
            builder.Position(new Vector3(1, 0, 0));
            builder.Position(new Vector3(0, 1, 0));
            builder.Position(new Vector3(1, 0, 0));
            builder.Position(new Vector3(1, 1, 0));
            builder.Position(new Vector3(0, 1, 0));

            var quad = builder.Complete();

            Assert.AreEqual(1, quad.Primitives.Count);
            Assert.AreEqual(6, quad.Primitives[0].GetIndexReader(StreamKey.Position).Count);
            Assert.AreEqual(4, quad.Primitives[0].BufferView.GetStreamReader<Vector3>(StreamKey.Position).Count);
        }
    }
}