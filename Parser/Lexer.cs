using System;

namespace Gone.Parser
{
    public class Lexer : yyParser.yyInput
    {
        int p;
        int tok;
        object val;

        readonly int length;
        readonly string code;

        public Lexer(string code)
        {
            this.code = code;
            length = code.Length;
            p = 0;
            tok = 0;
            val = null;
        }

        public bool advance()
        {
            //
            // Skip whitespace
            //
            while (p < length && char.IsWhiteSpace(code[p]))
                p++;

            //
            // Decide what to do
            //
            switch (code[p])
            {
                case '-':
                case '*':
                case '/':
                case '%':
                case '<':
                case '>':
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                    tok = code[p];
                    p++;
                    break;
                case '+':
                    if (p + 1 < length && code[p] == '=')
                    {
                        tok = TokenKind.OP_PLUSEQ;
                        p += 2;
                    }
                    else
                    {
                        tok = code[p];
                        p++;
                    }
                    break;
                default:
                    if (char.IsLetter(code[p]) || code[p] == '_')
                    {
                        var start = p;
                        var end = start + 1;
                        while (end < length && (char.IsLetterOrDigit(code[end]) || code[end] == '_'))
                        {
                            end++;
                        }
                        val = code.Substring(start, end - start);
                        tok = TokenKind.IDENTIFIER;
                        p = end;
                    }
                    else
                    {
                        throw new NotSupportedException($"Don't know how to parse `{code[p]}`");
                    }
                    break;
            }

            return true;
        }

        public int token() => tok;

        public object value() => val;
    }
}


