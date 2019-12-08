namespace TokenizerGenerator
{
    public class ConcatRule : Rule
    {
        public ConcatRule(Rule[] rules)
        {
            Rules = rules;
        }

        public Rule[] Rules { get; }

        public override RuleType Type => RuleType.Concat;
    }
}