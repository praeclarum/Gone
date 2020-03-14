using System;
using System.Collections.Generic;
using System.Text;

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

        public static Tuple<int, object>[] Tokenize(string code)
        {
            var r = new List<Tuple<int, object>>();
            var l = new Lexer(code);
            while (l.advance())
            {
                r.Add(Tuple.Create (l.token(), l.value()));
            }
            return r.ToArray();
        }

        public bool advance()
        {
            val = null;

            //
            // Skip whitespace
            //
            while (p < length && char.IsWhiteSpace(code[p]))
                p++;
            if (p >= length)
                return false;

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
                case '.':
                case ',':
                case ':':
                case ';':
                    tok = code[p];
                    p++;
                    break;
                case '+':
                    if (p + 1 < length && code[p + 1] == '=')
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
                case '\"':
                    {
                        var b = new StringBuilder();
                        var start = p;
                        var end = start + 1;
                        while (end < length && (code[end] != '\"'))
                        {
                            if (end + 1 < length && code[end] == '\\')
                            {
                                b.Append(code[end + 1]);
                                end += 2;
                            }
                            else
                            {
                                b.Append(code[end]);
                                end++;
                            }
                        }
                        end++;
                        val = b.ToString ();
                        tok = TokenKind.STRING_LITERAL;
                        p = end;
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


