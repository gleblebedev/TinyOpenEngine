namespace TokenizerGenerator
{
    public class TokenInfo
    {
        public TokenInfo(string name, Rule rule)
        {
            Name = name;
            Rule = rule;
        }

        public Rule Rule { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}