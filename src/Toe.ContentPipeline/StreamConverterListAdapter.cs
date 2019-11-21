using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class StreamConverterListAdapter<T> : StreamConverter<T>
    {
        private readonly List<T> list;

        public StreamConverterListAdapter(List<T> list)
        {
            this.list = list;
        }


        public override int Count
        {
            get { return list.Count; }
        }

        public override T this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }
    }
}