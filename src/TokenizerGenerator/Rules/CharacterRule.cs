namespace TokenizerGenerator.Rules
{
    public class CharacterRule : Rule
    {
        public CharacterRule(char character)
        {
            Character = character;
        }

        public char Character { get; }

        public override RuleType Type => RuleType.Char;

        public override LeadingChars EvaluateLeadingSymbols()
        {
            return new LeadingChars(false, new[] {Character});
        }

        public override string ToString()
        {
            return $"\'{Character}\'";
        }
    }
}