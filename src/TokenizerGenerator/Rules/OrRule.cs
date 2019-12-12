using System.Linq;

namespace TokenizerGenerator.Rules
{
    public class OrRule : Rule
    {
        public OrRule(Rule[] rules)
        {
            Rules = rules;
        }

        public Rule[] Rules { get; }

        public override RuleType Type => RuleType.Or;

        public override LeadingChars EvaluateLeadingSymbols()
        {
            var chars = new LeadingChars(false, Enumerable.Empty<char>());
            foreach (var rule in Rules) chars = chars.Union(rule.EvaluateLeadingSymbols());

            return chars;
        }

        public override string ToString()
        {
            return string.Join(" or ", Rules.Select(_ => $"({_})"));
        }
    }
}