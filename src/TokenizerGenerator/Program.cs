using System;

namespace TokenizerGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
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

            var candidates = new Rule[]
            {
                integer_literal,
                real_literal,
                regular_string_literal
            };
        }
    }
}
