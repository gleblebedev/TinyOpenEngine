using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IAssetContainer<T> : IReadOnlyDictionary<string, T>, IReadOnlyList<T> where T : IAsset
    {
        new int Count { get; }

        new IEnumerator<T> GetEnumerator();

        void Add(T asset);
    }
}