using System;
using System.IO;
using System.Text;
using TokenizerGenerator.Rules;

namespace TokenizerGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var new_line = Rule.Or( Rule.Concat('\r', Rule.Optional('\n')),
                Rule.Concat('\n', Rule.Optional('\r')));
            var whitespace_character = Rule.Or(' ', '\v', '\t', '\f');
            var whitespace = Rule.Concat(whitespace_character, Rule.Repeat(whitespace_character));
            var simple_escape_sequence = Rule.Or("\\'", "\\\"", "\\\\", "\\0", "\\a", "\\b", "\\f", "\\n", "\\r", "\\t", "\\v");
            var sign = Rule.Or('+', '-');
            var decimal_digit = Rule.Or('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            var decimal_digits = Rule.Concat(decimal_digit, Rule.Repeat(decimal_digit));
            var hex_digit = Rule.Or('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F');
            var hex_digits = Rule.Concat(hex_digit, Rule.Repeat(hex_digit));
            var hexadecimal_escape_sequence = Rule.Concat("\\x", hex_digit, Rule.Optional(hex_digit), Rule.Optional(hex_digit), Rule.Optional(hex_digit));
            var unicode_escape_sequence = Rule.Or(
                Rule.Concat("\\u", hex_digit, hex_digit, hex_digit, hex_digit),
                Rule.Concat("\\U", hex_digit, hex_digit, hex_digit, hex_digit, hex_digit, hex_digit, hex_digit, hex_digit));
            var exponent_part = Rule.Concat(Rule.Or('e', 'E'), Rule.Optional(sign), decimal_digits);
            var real_type_suffix = Rule.Or('f', 'F', 'd', 'D', 'm', 'M');
            var real_literal = Rule.Or(
                Rule.Concat(Rule.Optional(decimal_digits),'.',Rule.Optional(exponent_part),Rule.Optional(real_type_suffix)),
                Rule.Concat(decimal_digits, exponent_part, Rule.Optional(real_type_suffix)),
                Rule.Concat(decimal_digits, Rule.Optional(real_type_suffix))
                );
            var integer_type_suffix = Rule.Or('u', 'U', 'l', 'L', "ul", "uL", "Ul", "UL", "lu", "Lu", "lU", "LU");
            var decimal_integer_literal = Rule.Concat(decimal_digits, Rule.Optional(integer_type_suffix));
            var hexadecimal_integer_literal = Rule.Concat(Rule.Or("0x", "0X"), hex_digits, Rule.Optional(integer_type_suffix));
            var integer_literal = Rule.Or(
                decimal_integer_literal,
                hexadecimal_integer_literal
            );
            var single_regular_string_literal_character = Rule.AnyExcept('\"','\\');
            var regular_string_literal_character = Rule.Or(
                single_regular_string_literal_character,
                simple_escape_sequence,
                hexadecimal_escape_sequence,
                unicode_escape_sequence
                );
            var regular_string_literal = Rule.Concat('\"', Rule.Repeat(regular_string_literal_character), '\"');

            var name = "SimpleTokenizer2";
            using (var file = File.Open($"..\\..\\..\\..\\Toe.ContentPipeline.Tokenizer\\{name}.cs", FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var write = new StreamWriter(file, new UTF8Encoding(false)))
                {
                    Generate(write, "Toe.ContentPipeline.Tokenizer", name
                        , new TokenInfo("Integer", integer_literal)
                        , new TokenInfo("Float", real_literal)
                        , new TokenInfo("StringConstant", regular_string_literal)
                        , new TokenInfo("Whitespace", whitespace)
                        , new TokenInfo("NewLine", new_line));
                }
            }
        }

        private static void Generate(TextWriter writer, string @namespace, string name, params TokenInfo[] tokens)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("");
            writer.WriteLine($"namespace {@namespace}");
            writer.WriteLine("{");
            writer.WriteLine($"    public partial class {name} : AbstractTokenizer<{name}.TokenType>");
            writer.WriteLine("    {");
            writer.WriteLine("        public enum TokenType");
            writer.WriteLine("        {");
            foreach (var tokenInfo in tokens)
            {
                writer.WriteLine($"            {tokenInfo.Name},");
            }
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine($"        public {name}(ITokenObserver<TokenType> observer, ITokenEncoding encoding) : base(observer, encoding) {{ }}");
            writer.WriteLine($"        public {name}(ITokenObserver<TokenType> observer) : base(observer) {{ }}");
            writer.WriteLine($"        public {name}(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy) : base(observer, allocationStrategy) {{ }}");
            writer.WriteLine($"        public {name}(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy, ITokenEncoding encoding):base(observer, allocationStrategy, encoding) {{ }}");
            writer.WriteLine("");
            writer.WriteLine("        protected override int TryParseToken(in ReadOnlySpan<char> textSpan)");
            writer.WriteLine("        {");

            

            writer.WriteLine("            throw new NotImplementedException();");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }
    }
}
