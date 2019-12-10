using System.Linq;
using TokenizerGenerator.Rules;

namespace TokenizerGenerator
{
    public abstract class Rule
    {
        public abstract RuleType Type { get; }

        public static implicit operator Rule(char c)
        {
            return Char(c);
        }
        public static implicit operator Rule(string str)
        {
            return Str(str);
        }

        public static AnyExceptRule AnyExcept(params char[] exemptions)
        {
            return new AnyExceptRule(exemptions);
        }
        public static OrRule Or(params Rule[] rules)
        {
            return new OrRule(rules);
        }
        public static CharacterRule Char(char character)
        {
            return new CharacterRule(character);
        }
  
        public static Rule Str(string value)
        {
            if (value.Length == 1)
                return Char(value[0]);
            return new ConcatRule(value.Select(_=>Rule.Char(_)).ToArray());
        }
        public static ConcatRule Concat(params Rule[] rules)
        {
            return new ConcatRule(rules);
        }
        public static OptionalRule Optional(Rule rule)
        {
            return new OptionalRule(rule);
        }

        public static Rule Repeat(Rule rule)
        {
            return new RepeatRule(rule);
        }
    }
}