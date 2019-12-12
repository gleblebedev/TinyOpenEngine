using System.Collections.Generic;

namespace TokenizerGenerator
{
    public interface IGrammar
    {
        string Namespace { get; }
        string Name { get; }
        IList<TokenInfo> Tokens { get; }
    }
}