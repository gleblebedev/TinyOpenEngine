using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline.Transformations
{
    public class ContentVisitor : IContentTransformation
    {
        private readonly Dictionary<ICameraAsset, ICameraAsset> _cameras = new Dictionary<ICameraAsset, ICameraAsset>();
        private readonly Dictionary<IImageAsset, IImageAsset> _images = new Dictionary<IImageAsset, IImageAsset>();
        private readonly Dictionary<ILightAsset, ILightAsset> _lights = new Dictionary<ILightAsset, ILightAsset>();

        private readonly Dictionary<IMaterialAsset, IMaterialAsset> _materials =
            new Dictionary<IMaterialAsset, IMaterialAsset>();

        private readonly Dictionary<IMesh, ICollection<IMesh>> _meshes = new Dictionary<IMesh, ICollection<IMesh>>();

        public ContentContainer Content { get; private set; }

        public IContentContainer Apply(IContentContainer content)
        {
            if (Content != null) throw new InvalidOperationException("Can't run transformation more than once.");
            Content = new ContentContainer();
            Content.Images.AddRange(content.Images.Select(_ => Apply(_, _images, Apply)).ToList());
            Content.Materials.AddRange(content.Materials.Select(_ => Apply(_, _materials, Apply)).ToList());
            Content.Meshes.AddRange(content.Meshes.SelectMany(_ => Apply(_, _meshes, Apply)).ToList());
            Content.Cameras.AddRange(content.Cameras.Select(_ => Apply(_, _cameras, Apply)).ToList());
            Content.Lights.AddRange(content.Lights.Select(_ => Apply(_, _lights, Apply)).ToList());
            Content.Scenes.AddRange(content.Scenes.SelectMany(Apply).ToList());
            return Content;
        }

        public virtual IImageAsset Apply(IImageAsset image)
        {
            return image;
        }

        public virtual IMaterialAsset Apply(IMaterialAsset material)
        {
            return material;
        }

        private static IEnumerable<T> Apply<T>(T item, Dictionary<T, ICollection<T>> map,
            Func<T, IEnumerable<T>> transform)
        {
            var res = transform(item).ToList();
            map[item] = res;
            return res;
        }

        private static T Apply<T>(T item, Dictionary<T, T> map,
            Func<T, T> transform)
        {
            var res = transform(item);
            map[item] = res;
            return res;
        }

        public virtual IEnumerable<IMesh> Apply(IMesh mesh)
        {
            yield return mesh;
        }

        public virtual ICameraAsset Apply(ICameraAsset camera)
        {
            return camera;
        }

        public virtual ILightAsset Apply(ILightAsset camera)
        {
            return camera;
        }

        public virtual IEnumerable<NodeAsset> Apply(NodeAsset parentNode, INodeAsset node)
        {
            NodeAsset transformedNode = null;
            if (node.Mesh != null)
            {
                var transformedMaterials = node.Mesh.Materials.Select(Map).ToList();
                foreach (var transformedMesh in Map(node.Mesh.Mesh))
                {
                    if (transformedNode != null)
                        yield return transformedNode;
                    transformedNode = CreateNodeCopy(parentNode, node);
                    transformedNode.Mesh = new MeshInstance(transformedMesh, transformedMaterials);
                }
            }

            transformedNode = transformedNode ?? CreateNodeCopy(parentNode, node);
            if (node.Camera != null) transformedNode.Camera = Map(node.Camera);
            if (node.Light != null) transformedNode.Light = Map(node.Light);
            yield return transformedNode;
        }

        private static NodeAsset CreateNodeCopy(NodeAsset parentNode, INodeAsset node)
        {
            NodeAsset transformedNode;
            transformedNode = new NodeAsset(node.Id);
            transformedNode.Transform.CopyFrom(node.Transform);
            transformedNode.Parent = parentNode;
            return transformedNode;
        }

        public IEnumerable<IMesh> Map(IMesh mesh)
        {
            if (_meshes.TryGetValue(mesh, out var result))
                return result;
            return Enumerable.Empty<IMesh>();
        }

        public IMaterialAsset Map(IMaterialAsset mesh)
        {
            if (_materials.TryGetValue(mesh, out var result))
                return result;
            return null;
        }

        public IImageAsset Map(IImageAsset mesh)
        {
            if (_images.TryGetValue(mesh, out var result))
                return result;
            return null;
        }

        public ICameraAsset Map(ICameraAsset camera)
        {
            if (_cameras.TryGetValue(camera, out var result))
                return result;
            return null;
        }

        public ILightAsset Map(ILightAsset light)
        {
            if (_lights.TryGetValue(light, out var result))
                return result;
            return null;
        }

        public virtual IEnumerable<ISceneAsset> Apply(ISceneAsset scene)
        {
            var transformedScene = new SceneAsset(scene.Id);
            foreach (var node in scene.ChildNodes)
            {
                NodeAsset parent = null;
                foreach (var transformedNode in Apply(null, node))
                {
                    transformedScene.Add(transformedNode);
                    parent = transformedNode;
                }

                if (parent != null)
                    foreach (var childNode in node.ChildNodes)
                        ApplyOnSubtree(parent, childNode);
            }

            yield return transformedScene;
        }

        private void ApplyOnSubtree(NodeAsset transformedParent, INodeAsset node)
        {
            NodeAsset parent = null;
            foreach (var transformedNode in Apply(transformedParent, node)) parent = transformedNode;
            if (parent != null)
                foreach (var childNode in node.ChildNodes)
                    ApplyOnSubtree(parent, childNode);
        }
    }
}