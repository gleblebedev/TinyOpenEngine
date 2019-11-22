using System;

namespace Toe.ContentPipeline
{
    public abstract class AbstractAsset : IAsset
    {
        private string _id;

        public AbstractAsset(string id)
        {
            _id = id == string.Empty ? null : id;
        }

        public AbstractAsset()
        {
        }

        public string Id
        {
            get => _id;
            set
            {
                if (_id != null)
                {
                    if (_id == value)
                        return;
                    throw new InvalidOperationException("Can't change id. You can only set id once.");
                }

                _id = value == string.Empty ? null : value;
            }
        }

        public override string ToString()
        {
            return $"({GetType().Name}){Id}";
        }
    }
}