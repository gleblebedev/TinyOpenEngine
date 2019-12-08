namespace TokenizerGenerator
{
    public class OrRule : Rule
    {
        public OrRule(Rule[] rules)
        {
            Rules = rules;
        }

        public Rule[] Rules { get; }

        public override RuleType Type => RuleType.Or;
    }
}