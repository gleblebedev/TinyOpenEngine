using System.IO;
using System.Text;

namespace TokenizerGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var sign = Rule.Or('+', '-');

            //var exponent_part = Rule.Concat(Rule.Or('e', 'E'), Rule.Optional(sign), Rule.DecimalDigits);
            //var real_type_suffix = Rule.Or('f', 'F', 'd', 'D', 'm', 'M');
            //var real_literal = Rule.Or(
            //    Rule.Concat(Rule.Optional(Rule.DecimalDigits),'.',Rule.Optional(exponent_part),Rule.Optional(real_type_suffix)),
            //    Rule.Concat(Rule.DecimalDigits, exponent_part, Rule.Optional(real_type_suffix)),
            //    Rule.Concat(Rule.DecimalDigits, Rule.Optional(real_type_suffix))
            //    );
            //var integer_type_suffix = Rule.Or('u', 'U', 'l', 'L', "ul", "uL", "Ul", "UL", "lu", "Lu", "lU", "LU");
            //var decimal_integer_literal = Rule.Concat(Rule.DecimalDigits, Rule.Optional(integer_type_suffix));
            //var hexadecimal_integer_literal = Rule.Concat(Rule.Or("0x", "0X"), Rule.HexDigits, Rule.Optional(integer_type_suffix));
            //var integer_literal = Rule.Or(
            //    decimal_integer_literal,
            //    hexadecimal_integer_literal
            //);

            var grammar = new SimpleTokenizer2();

            using (var file = File.Open($"..\\..\\..\\..\\Toe.ContentPipeline.Tokenizer\\{grammar.Name}.cs",
                FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using var write = new StreamWriter(file, new UTF8Encoding(false));
                var codeGen = new CodeGen(write, grammar);
                codeGen.Generate();
            }
        }
    }
}