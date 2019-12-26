using System;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline
{
    public struct ImmutableId
    {
        private string _id;

        public ImmutableId(string id)
        {
            _id = EvaluateId(id);
        }

        public string Id
        {
            get => _id;
            set
            {
                var newId = EvaluateId(value);
                if (_id == newId) return;

                if (_id != null) throw new InvalidOperationException("Can't change id. You can only set id once.");

                _id = newId;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string EvaluateId(string id)
        {
            return id == string.Empty ? null : id;
        }

        public override string ToString()
        {
            return _id;
        }
    }
}