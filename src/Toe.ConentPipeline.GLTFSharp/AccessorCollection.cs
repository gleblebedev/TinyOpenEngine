using System;
using System.Collections;
using System.Collections.Generic;
using SharpGLTF.Schema2;

namespace Toe.ConentPipeline.GLTFSharp
{
    public class AccessorCollection:IEnumerable<KeyValuePair<string, Accessor>>
    {
        public int VertexCount { get; private set; }
        Dictionary<string, Accessor> _accessors = new Dictionary<string, Accessor>();
        public void Add(string key, Accessor accessor)
        {
            if (_accessors.Count == 0)
            {
                VertexCount = accessor.Count;
            }
            else
            {
                if (VertexCount != accessor.Count)
                    throw new FormatException($"Accessor {key} has {accessor.Count} items while previous accessors had {VertexCount} items");
            }
            if (_accessors.TryGetValue(key, out var existingAccessor))
            {
                if (existingAccessor != accessor)
                {
                    throw new NotImplementedException($"Different accessors for the {key} stream are not supported yet");
                }
            }
            else
            {
                _accessors.Add(key, accessor);
            }
        }

        public IEnumerator<KeyValuePair<string, Accessor>> GetEnumerator()
        {
            return ((IDictionary<string, Accessor>)_accessors).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}