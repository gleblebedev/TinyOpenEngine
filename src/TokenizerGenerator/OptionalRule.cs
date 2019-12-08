namespace TokenizerGenerator
{
    public class OptionalRule : Rule
    {
        public OptionalRule(Rule rule)
        {
            Rule = rule;
        }

        public Rule Rule { get; }

        public override RuleType Type => RuleType.Optional;
    }
}