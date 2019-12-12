using System.Linq;
using TokenizerGenerator.Rules;

namespace TokenizerGenerator
{
    public abstract class Rule
    {
        public static readonly Rule DecimalDigit;
        public static readonly Rule DecimalDigits;
        public static readonly Rule HexDigit;
        public static readonly Rule HexDigits;
        public static readonly Rule SimpleEscapeSequence;
        public static readonly Rule WhitespaceCharacter;
        public static readonly Rule Whitespace;
        public static readonly Rule NewLine;
        public static readonly Rule RegularStringLiteral;

        static Rule()
        {
            DecimalDigit = Or('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            DecimalDigits = Concat(DecimalDigit, Repeat(DecimalDigit));
            HexDigit = Or('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C',
                'D', 'E', 'F');
            HexDigits = Concat(HexDigit, Repeat(HexDigit));
            SimpleEscapeSequence = Or("\\'", "\\\"", "\\\\", "\\0", "\\a", "\\b", "\\f", "\\n", "\\r", "\\t", "\\v");
            WhitespaceCharacter = Or(' ', '\v', '\t', '\f');
            Whitespace = Concat(WhitespaceCharacter, Repeat(WhitespaceCharacter));
            NewLine = Or(Concat('\r', Optional('\n')),
                Concat('\n', Optional('\r')));
            var hexadecimal_escape_sequence =
                Concat("\\x", HexDigit, Optional(HexDigit), Optional(HexDigit), Optional(HexDigit));
            var unicode_escape_sequence = Or(
                Concat("\\u", HexDigit, HexDigit, HexDigit, HexDigit),
                Concat("\\U", HexDigit, HexDigit, HexDigit, HexDigit, HexDigit, HexDigit, HexDigit, HexDigit));
            var single_regular_string_literal_character = AnyExcept('\"', '\\');
            var regular_string_literal_character = Or(
                single_regular_string_literal_character,
                SimpleEscapeSequence,
                hexadecimal_escape_sequence,
                unicode_escape_sequence
            );
            RegularStringLiteral = Concat('\"', Repeat(regular_string_literal_character), '\"');
        }

        public abstract RuleType Type { get; }

        public static implicit operator Rule(char c)
        {
            return Char(c);
        }

        public static implicit operator Rule(string str)
        {
            return Str(str);
        }

        public abstract LeadingChars EvaluateLeadingSymbols();

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
            return new ConcatRule(value.Select(_ => Char(_)).ToArray());
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