using System.Linq;

namespace TokenizerGenerator.Rules
{
    public class AnyExceptRule : Rule
    {
        public AnyExceptRule(char[] exemptions)
        {
            LeadingChars = new LeadingChars(true, exemptions);
        }

        public LeadingChars LeadingChars { get; }

        public override RuleType Type => RuleType.AnyExcept;

        public override LeadingChars EvaluateLeadingSymbols()
        {
            return LeadingChars;
        }

        public override string ToString()
        {
            return $"any except {string.Join(",", LeadingChars.Chars.Select(_ => "\'" + _ + "\'"))}";
        }
    }
}