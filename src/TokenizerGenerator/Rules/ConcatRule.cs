using System;
using System.Collections.Generic;
using System.Linq;

namespace TokenizerGenerator.Rules
{
    public class ConcatRule : Rule
    {
        public ConcatRule(Rule[] rules)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));
            if (rules.Length == 0)
                throw new ArgumentException($"{nameof(rules)} array can't be empty");
            Rules = rules;
        }

        public Rule[] Rules { get; }

        public override RuleType Type => RuleType.Concat;

        public override LeadingChars EvaluateLeadingSymbols()
        {
            var chars = new LeadingChars(false, Enumerable.Empty<char>());

            var index = 0;
            while (index < Rules.Length)
            {
                chars = chars.Union(Rules[index].EvaluateLeadingSymbols());
                if (Rules[index].Type != RuleType.Optional)
                    break;
                ++index;
            }

            return chars;
        }

        public override string ToString()
        {
            return string.Join(" then ", (IEnumerable<Rule>) Rules);
        }
    }
}