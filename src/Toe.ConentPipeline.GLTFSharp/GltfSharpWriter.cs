using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SharpGLTF.Geometry;
using SharpGLTF.Scenes;
using Toe.ContentPipeline;

namespace Toe.ConentPipeline.GLTFSharp
{
    public class GltfSharpWriter : IFileWriter
    {
        public Task WriteAsync(Stream stream, IContentContainer content)
        {
            return Task.Run(() =>
            {
                var scenes = new List<SceneBuilder>();
                //var meshes = BuildMeshes(content.Meshes);
                
                    foreach (var sceneAsset in content.Scenes)
                {
                    var scene = new SceneBuilder();
                    scenes.Add(scene);
                    foreach (var node in sceneAsset.ChildNodes)
                    {
                        var nodeBuilder = new NodeBuilder(node.Id);
                        nodeBuilder.LocalTransform = node.Transform.Matrix;
                        if (node.Mesh != null)
                        {
                            //scene.AddMesh()
                        }
                    }
                }

                var modelRoot = SceneBuilder.ToSchema2(scenes);
                modelRoot.WriteGLB(stream);
            });
        }

        //private Dictionary<IMesh, IMeshBuilder<>> BuildMeshes(IAssetContainer<IMesh> contentMeshes)
        //{
            
        //}
    }
}