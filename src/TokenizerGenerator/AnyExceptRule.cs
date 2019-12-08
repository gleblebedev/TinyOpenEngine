namespace TokenizerGenerator
{
    public class AnyExceptRule : Rule
    {
        public AnyExceptRule(char[] exemptions)
        {
            Exemptions = exemptions;
        }

        public char[] Exemptions { get; }

        public override RuleType Type => RuleType.AnyExcept;
    }
}