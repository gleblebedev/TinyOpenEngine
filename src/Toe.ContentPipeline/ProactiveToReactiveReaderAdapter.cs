namespace Toe.ContentPipeline
{
    public class ProactiveToReactiveReaderAdapter : IReactiveReader
    {
        private readonly IStreamReader _reader;

        //private readonly List<byte> _buffer;

        public ProactiveToReactiveReaderAdapter(IStreamReader reader)
        {
            _reader = reader;
            //_buffer = new List<byte>(512);
        }

        //public void OnNext(byte data)
        //{
        //    _buffer.Add(data);
        //}

        //public void OnError(Exception exception)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnCompleted()
        //{
        //    if (SceneItem == null)
        //        return;
        //    var scene = _reader.Read(new MemoryStream(_buffer.ToArray()));
        //    foreach (var effect in scene.Effects)
        //    {
        //        SceneItem(this, new SceneItemEventArgs(effect));
        //    }
        //    foreach (var material in scene.Materials)
        //    {
        //        SceneItem(this, new SceneItemEventArgs(material));
        //    }
        //    foreach (var geometry in scene.Geometries)
        //    {
        //        SceneItem(this, new SceneItemEventArgs(geometry));
        //    }
        //}

        //public event EventHandler<SceneItemEventArgs> SceneItem;

        //public void Dispose()
        //{
        //    _reader.Dispose();
        //}
    }
}