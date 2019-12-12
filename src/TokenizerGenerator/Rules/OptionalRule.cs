namespace TokenizerGenerator.Rules
{
    public class OptionalRule : Rule
    {
        public OptionalRule(Rule rule)
        {
            Rule = rule;
        }

        public Rule Rule { get; }

        public override RuleType Type => RuleType.Optional;

        public override LeadingChars EvaluateLeadingSymbols()
        {
            return Rule.EvaluateLeadingSymbols();
        }

        public override string ToString()
        {
            return $"maybe {Rule}";
        }
    }
}