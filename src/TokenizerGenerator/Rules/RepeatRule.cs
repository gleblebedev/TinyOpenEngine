namespace TokenizerGenerator.Rules
{
    public class RepeatRule : Rule
    {
        public RepeatRule(Rule rule)
        {
            Rule = rule;
        }

        public Rule Rule { get; }

        public override RuleType Type => RuleType.Repeat;
    }
}