using System;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class ReactiveToProactiveReaderAdapter : IStreamReader
    {
        private readonly IReactiveReader _reader;

        public ReactiveToProactiveReaderAdapter(IReactiveReader reader)
        {
            _reader = reader;
        }

        //public IContentContainer Read(Stream stream)
        //{
        //    var scene = new Scene<>();
        //    int b;
        //    EventHandler<SceneItemEventArgs> readerOnSceneItem = (s, a) => AddSeneItem(scene, a.Item);
        //    _reader.SceneItem += readerOnSceneItem;
        //    var line = 0;
        //    var column = 0;
        //    var filePosition = 0;
        //    try
        //    {
        //        try
        //        {
        //            try
        //            {
        //                while ((b = stream.ReadByte()) >= 0)
        //                {
        //                    _reader.OnNext((byte)b);
        //                    if (b == '\n')
        //                    {
        //                        ++line;
        //                        column = 0;
        //                    }
        //                    else if (b == '\r')
        //                    {
        //                    }
        //                    else
        //                    {
        //                        ++column;
        //                    }
        //                    ++filePosition;
        //                }
        //                _reader.OnCompleted();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new ParsingException(ex, line, column, filePosition);
        //            }
        //        }
        //        catch (ParsingException ex)
        //        {
        //            _reader.OnError(ex);
        //        }
        //        return scene;
        //    }
        //    finally
        //    {
        //        _reader.SceneItem -= readerOnSceneItem;
        //    }
        //}

        //public void Dispose()
        //{
        //    _reader.Dispose();
        //}

        //private void AddSeneItem(Scene scene, ISceneItem item)
        //{
        //    var mesh = item as IMesh;
        //    if (mesh != null)
        //    {
        //        scene.Geometries.Add(mesh);
        //    }
        //    var material = item as IMaterial;
        //    if (material != null)
        //    {
        //        scene.Materials.Add(material);
        //    }
        //    var effect = item as IEffect;
        //    if (effect != null)
        //    {
        //        scene.Effects.Add(effect);
        //    }
        //    var image = item as IImage;
        //    if (image != null)
        //    {
        //        scene.Images.Add(image);
        //    }
        //    var animation = item as IAnimation;
        //    if (animation != null)
        //    {
        //        scene.Animations.Add(animation);
        //    }
        //    var node = item as INode;
        //    if (node != null)
        //    {
        //        scene.Nodes.Add(node);
        //    }
        //}
        public Task<IContentContainer> ReadAsync(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}