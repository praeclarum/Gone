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
            var r = yyparse(lexer);
            return (Syntax.TopLevelDecl[])r;
        }
    }
}
