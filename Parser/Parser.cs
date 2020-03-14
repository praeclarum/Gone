using System;

namespace Gone.Parser
{
    public partial class GoParser
    {
        int yacc_verbose_flag = 0;

        public GoParser()
        {
        }

        public Syntax.TopLevelDecl[] Parse(string code)
        {
            var lexer = new Lexer(code);
            try
            {
                var r = yyparse(lexer);
                return (Syntax.TopLevelDecl[])r;
            }
            catch (yyParser.yyException ex)
            {
                var index = lexer.CurrentIndex;
                var startIndex = Math.Max (0, index - 10);
                var endIndex = Math.Min(code.Length, index + 10);
                var subcode = code.Substring(startIndex, endIndex - startIndex);
                throw new Exception($"Bad syntax near:\n{subcode}", ex);
            }
        }
    }
}
