namespace TokenizerGenerator
{
    public class CharacterRule : Rule
    {
        public CharacterRule(char character)
        {
            Character = character;
        }

        public char Character { get; }

        public override RuleType Type => RuleType.Char;
    }
}