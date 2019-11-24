using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Toe.SceneGraph
{
    [TestFixture]
    public class SceneTestFixture
    {

        [Test]
        public void CreateNode_NoParent_NodeAddedToScene()
        {
            var scene = new Scene<int>();

            var node = scene.CreateNode(null, 0);

            Assert.AreEqual(node, scene.Children.First());
        }

        [Test]
        public void CreateNode_ParentNode_NodeAddedToParent()
        {
            var scene = new Scene<int>();
            var parentNode = scene.CreateNode(null, 0);

            var node = scene.CreateNode(parentNode, 0);

            Assert.AreEqual(node, parentNode.Children.First());
            Assert.IsFalse(scene.Children.Any(_=>_ == node));
        }

        [Test]
        public void CreateNode_NoParentThenAttachedToParent_NodeAddedToParent()
        {
            var scene = new Scene<int>();
            var parentNode = scene.CreateNode(null, 0);
            var node = scene.CreateNode(null, 0);

            node.Parent = parentNode;

            Assert.AreEqual(node, parentNode.Children.First());
            Assert.IsFalse(scene.Children.Any(_ => _ == node));
        }

    }
}
