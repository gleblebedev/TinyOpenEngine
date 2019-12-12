using System.IO;
using System.Linq;

namespace TokenizerGenerator
{
    public class CodeGen
    {
        private readonly IGrammar _grammar;
        private readonly StreamWriter _writer;

        public CodeGen(StreamWriter writer, IGrammar grammar)
        {
            _writer = writer;
            _grammar = grammar;
        }

        public void Generate()
        {
            _writer.WriteLine("using System;");
            _writer.WriteLine("");
            _writer.WriteLine($"namespace {_grammar.Namespace}");
            _writer.WriteLine("{");
            _writer.WriteLine(
                $"    public partial class {_grammar.Name} : AbstractTokenizer<{_grammar.Name}.TokenType>");
            _writer.WriteLine("    {");
            _writer.WriteLine("        public enum TokenType");
            _writer.WriteLine("        {");
            foreach (var tokenInfo in _grammar.Tokens) _writer.WriteLine($"            {tokenInfo.Name},");
            _writer.WriteLine("        }");
            _writer.WriteLine("");
            _writer.WriteLine(
                $"        public {_grammar.Name}(ITokenObserver<TokenType> observer, ITokenEncoding encoding) : base(observer, encoding) {{ }}");
            _writer.WriteLine(
                $"        public {_grammar.Name}(ITokenObserver<TokenType> observer) : base(observer) {{ }}");
            _writer.WriteLine(
                $"        public {_grammar.Name}(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy) : base(observer, allocationStrategy) {{ }}");
            _writer.WriteLine(
                $"        public {_grammar.Name}(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy, ITokenEncoding encoding):base(observer, allocationStrategy, encoding) {{ }}");
            _writer.WriteLine("");
            _writer.WriteLine("        protected override int TryParseToken(in ReadOnlySpan<char> textSpan)");
            _writer.WriteLine("        {");

            var res = _grammar.Tokens.Select(_ => _.Rule.EvaluateLeadingSymbols()).ToList();

            _writer.WriteLine("            throw new NotImplementedException();");
            _writer.WriteLine("        }");
            _writer.WriteLine("    }");
            _writer.WriteLine("}");
        }
    }
}