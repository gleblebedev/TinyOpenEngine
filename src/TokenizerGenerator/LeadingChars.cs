using System.Collections.Generic;
using System.Linq;

namespace TokenizerGenerator
{
    public class LeadingChars
    {
        public LeadingChars(bool isExclusion, IEnumerable<char> chars)
        {
            IsExclusion = isExclusion;
            Chars = new HashSet<char>(chars);
        }

        private bool IsExclusion { get; }
        public HashSet<char> Chars { get; }

        public LeadingChars Union(LeadingChars other)
        {
            if (!IsExclusion && !other.IsExclusion) return new LeadingChars(IsExclusion, Chars.Concat(other.Chars));
            if (IsExclusion && other.IsExclusion)
            {
                var hashSet = new HashSet<char>(Chars);
                hashSet.IntersectWith(other.Chars);
                return new LeadingChars(IsExclusion, hashSet);
            }

            if (IsExclusion && !other.IsExclusion)
            {
                var hashSet = new HashSet<char>(Chars);
                foreach (var c in other.Chars) hashSet.Remove(c);
                return new LeadingChars(true, hashSet);
            }

            return other.Union(this);
        }

        public override string ToString()
        {
            if (IsExclusion)
                return "except " + string.Join(", ", Chars.Select(_ => "\'" + _ + "\'"));
            return string.Join(", ", Chars.Select(_ => "\'" + _ + "\'"));
        }
    }
}