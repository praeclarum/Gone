using System;
using System.Collections.Generic;
namespace Gone.Parser
{
    public partial class GoParser
    {
        int yacc_verbose_flag = 0;

        public GoParser()
        {
        }

        public Syntax.SourceFile Parse(string code)
        {
            var lexer = new Lexer(code);
            try
            {
                var r = yyparse(lexer);
                return (Syntax.SourceFile)r;
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

        T[] ToArray<T>(object list)
        {
            return ((List<T>)list).ToArray ();
        }

        List<T> EmptyList<T>()
        {
            return new List<T> ();
        }

        List<T> NewList<T>(object item)
        {
            return new List<T> { (T)item };
        }

        List<T> AppendList<T>(object list, object item)
        {
            var l = (List<T>)list;
            l.Add((T)item);
            return l;
        }
    }
}
