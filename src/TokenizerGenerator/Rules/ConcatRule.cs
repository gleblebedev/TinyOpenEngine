using System;

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
    }
}