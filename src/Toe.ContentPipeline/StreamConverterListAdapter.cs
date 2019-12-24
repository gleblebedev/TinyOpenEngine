using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class StreamConverterListAdapter<T> : StreamConverter<T>
    {
        private readonly IList<T> list;

        public StreamConverterListAdapter(IList<T> list)
        {
            this.list = list;
        }


        public override int Count => list.Count;

        public override T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
    }
}