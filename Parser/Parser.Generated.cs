// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 2 "Parser.jay"

using System;


#nullable disable

namespace Gone.Parser
{
	/// <summary>
	///    The Go Parser
	///    https://golang.org/ref/spec#Pointer_types
	/// </summary>
	public partial class GoParser
	{

		
#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = System.Console.Out;

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /* An EOF token */
  public int eof_token;

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
//t  internal yydebug.yyDebug debug;

  protected const int yyFinal = 2;
//t // Put this array into a separate class so it is only initialized if debugging is actually used
//t // Use MarshalByRefObject to disable inlining
//t class YYRules : MarshalByRefObject {
//t  public static readonly string [] yyRule = {
//t    "$accept : source_file",
//t    "source_file : package_clause import_decl_list top_level_decl_list",
//t    "source_file : package_clause import_decl_list",
//t    "source_file : package_clause top_level_decl_list",
//t    "source_file : package_clause",
//t    "package_clause : PACKAGE IDENTIFIER",
//t    "import_decl_list : import_decl",
//t    "import_decl_list : import_decl_list import_decl",
//t    "import_decl : IMPORT import_spec",
//t    "import_decl : IMPORT '(' import_spec_list ')'",
//t    "import_spec_list : import_spec",
//t    "import_spec_list : import_spec_list ';' import_spec",
//t    "import_spec : package_name_list import_path",
//t    "import_path : STRING_LITERAL",
//t    "package_name_list : package_name",
//t    "package_name_list : package_name_list '.' package_name",
//t    "package_name : IDENTIFIER",
//t    "top_level_decl_list : top_level_decl",
//t    "top_level_decl_list : top_level_decl_list top_level_decl",
//t    "top_level_decl : function_decl",
//t    "function_decl : FUNC IDENTIFIER function_signature function_body",
//t    "function_decl : FUNC IDENTIFIER function_signature",
//t    "function_body : block",
//t    "function_signature : parameters result",
//t    "function_signature : parameters",
//t    "result : parameters",
//t    "result : type",
//t    "parameters : '(' parameter_list ')'",
//t    "parameters : '(' ')'",
//t    "parameter_list : parameter_decl",
//t    "parameter_list : parameter_list ',' parameter_decl",
//t    "parameter_decl : identifier_list OP_ELLIPSIS type",
//t    "parameter_decl : identifier_list type",
//t    "parameter_decl : type",
//t    "identifier_list : IDENTIFIER",
//t    "identifier_list : identifier_list ',' IDENTIFIER",
//t    "block : '{' statement_list '}'",
//t    "block : '{' '}'",
//t    "statement_list : statement",
//t    "statement_list : statement_list ';' statement",
//t    "statement : simple_stmt",
//t    "simple_stmt : expression_stmt",
//t    "expression_stmt : expression",
//t    "expression : unary_expr",
//t    "unary_expr : primary_expr",
//t    "primary_expr : primary_expr arguments",
//t    "arguments : '(' inner_arguments ')'",
//t    "arguments : '(' ')'",
//t    "inner_arguments : expression_list",
//t    "expression_list : expression",
//t    "expression_list : expression_list ',' expression",
//t    "type : type_name",
//t    "type_name : TYPE_IDENTIFIER",
//t  };
//t public static string getRule (int index) {
//t    return yyRule [index];
//t }
//t}
  protected static readonly string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    "'('","')'",null,null,"','",null,"'.'",null,null,null,null,null,null,
    null,null,null,null,null,null,"';'",null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,"'{'",null,"'}'",null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,"IDENTIFIER",
    "TYPE_IDENTIFIER","INTEGER_LITERAL","FLOATING_LITERAL","RUNE_LITERAL",
    "STRING_LITERAL","BREAK","DEFAULT","FUNC","INTERFACE","SELECT","CASE",
    "DEFER","GO","MAP","STRUCT","CHAN","ELSE","GOTO","PACKAGE","SWITCH",
    "CONST","FALLTHROUGH","IF","RANGE","TYPE","CONTINUE","FOR","IMPORT",
    "RETURN","VAR","OP_PLUSEQ","OP_ELLIPSIS",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
  public static string yyname (int token) {
    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
    string name;
    if ((name = yyNames[token]) != null) return name;
    return "[unknown]";
  }

  int yyExpectingState;
  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected int [] yyExpectingTokens (int state){
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];
    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    int [] result = new int [len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = token;
    return result;
  }
  protected string[] yyExpecting (int state) {
    int [] tokens = yyExpectingTokens (state);
    string [] result = new string[tokens.Length];
    for (int n = 0; n < tokens.Length;  n++)
      result[n++] = yyNames[tokens [n]];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
//t    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

	static int[] global_yyStates;
	static object[] global_yyVals;
	protected bool use_global_stacks;
	object[] yyVals;					// value stack
	object yyVal;						// value stack ptr
	int yyToken;						// current input
	int yyTop;

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;		// initial size
    int yyState = 0;                   // state stack ptr
    int [] yyStates;               	// state stack 
    yyVal = null;
    yyToken = -1;
    int yyErrorFlag = 0;				// #tks to shift
	if (use_global_stacks && global_yyStates != null) {
		yyVals = global_yyVals;
		yyStates = global_yyStates;
   } else {
		yyVals = new object [yyMax];
		yyStates = new int [yyMax];
		if (use_global_stacks) {
			global_yyVals = yyVals;
			global_yyStates = yyStates;
		}
	}

    /*yyLoop:*/ for (yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        global::System.Array.Resize (ref yyStates, yyStates.Length+yyMax);
        global::System.Array.Resize (ref yyVals, yyVals.Length+yyMax);
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
//t      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ while (true) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
//t            if (debug != null)
//t              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              yyExpectingState = yyState;
              // yyerror(String.Format ("syntax error, got token `{0}'", yyname (yyToken)), yyExpecting(yyState));
//t              if (debug != null) debug.error("syntax error");
              if (yyToken == 0 /*eof*/ || yyToken == eof_token) throw new yyParser.yyUnexpectedEof ();
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += TokenKind.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == TokenKind.yyErrorCode) {
//t                  if (debug != null)
//t                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
//t                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
//t              if (debug != null) debug.reject();
              throw new yyParser.yyException("irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
//t                if (debug != null) debug.reject();
                throw new yyParser.yyException("irrecoverable syntax error at end-of-file");
              }
//t              if (debug != null)
//t                debug.discard(yyState, yyToken, yyname(yyToken),
//t  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
//t        if (debug != null)
//t          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyV > yyTop ? null : yyVals[yyV]; // yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
//t          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
//t            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
//t        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: ;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: ;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

/*
 All more than 3 lines long rules are wrapped into a method
*/
#line default
   static readonly short [] yyLhs  = {              -1,
    0,    0,    0,    0,    1,    2,    2,    4,    4,    6,
    6,    5,    8,    7,    7,    9,    3,    3,   10,   11,
   11,   13,   12,   12,   16,   16,   15,   15,   18,   18,
   19,   19,   19,   20,   20,   14,   14,   21,   21,   22,
   23,   24,   25,   26,   27,   28,   28,   29,   30,   30,
   17,   31,
  };
   static readonly short [] yyLen = {           2,
    3,    2,    2,    1,    2,    1,    2,    2,    4,    1,
    3,    2,    1,    1,    3,    1,    1,    2,    1,    4,
    3,    1,    2,    1,    1,    1,    3,    2,    1,    3,
    3,    2,    1,    1,    3,    3,    2,    1,    3,    1,
    1,    1,    1,    1,    2,    3,    2,    1,    1,    3,
    1,    1,
  };
   static readonly short [] yyDefRed = {            0,
    0,    0,    0,    5,    0,    0,    0,    0,    6,   17,
   19,    0,   16,    0,    8,    0,   14,    0,    7,   18,
    0,    0,    0,   10,    0,   13,    0,   12,   34,   52,
   28,   33,    0,   29,    0,   51,    0,   20,   22,   25,
   23,   26,    9,    0,   15,   27,    0,    0,    0,   32,
   37,    0,   38,   40,   41,   42,   43,    0,   11,   30,
   31,   35,    0,   36,    0,   45,   39,   47,   49,    0,
    0,   46,    0,   50,
  };
  protected static readonly short [] yyDgoto  = {             2,
    3,    7,    8,    9,   15,   25,   16,   28,   17,   10,
   11,   22,   38,   39,   23,   41,   32,   33,   34,   35,
   52,   53,   54,   55,   56,   57,   58,   66,   70,   71,
   36,
  };
  protected static readonly short [] yySindex = {         -256,
 -235,    0, -254,    0, -231,  -33, -254, -238,    0,    0,
    0,  -12,    0, -228,    0,  -42,    0, -238,    0,    0,
  -35,  -93,  -37,    0,  -27,    0, -228,    0,    0,    0,
    0,    0,  -25,    0,  -44,    0,  -92,    0,    0,    0,
    0,    0,    0, -228,    0,    0, -240, -224, -222,    0,
    0,  -50,    0,    0,    0,    0,    0,   -2,    0,    0,
    0,    0,    0,    0,   -5,    0,    0,    0,    0,    3,
   -3,    0,    0,    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,   39,    0,    0,    0,   42,   43,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   45,    0,    0,
    0,    2,    1,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  -36,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    5,    0,    0,    0,
  };
  protected static readonly short [] yyGindex = {            0,
    0,    0,   40,   41,   -4,    0,    0,    0,   22,    7,
    0,    0,    0,    0,   27,    0,  -11,    0,    4,    0,
    0,  -10,    0,    0,  -52,    0,    0,    0,    0,    0,
    0,
  };
  protected static readonly short [] yyTable = {            49,
   24,   21,   21,   27,   44,   31,   14,   44,   63,   24,
    5,   42,   69,   43,   20,   46,   29,   30,   47,    1,
   74,    4,   44,   50,   20,   12,    5,   21,   13,   37,
    6,   44,   51,   30,   62,   68,   61,   65,    4,   59,
   73,    2,    3,   72,    1,   48,   18,   19,   45,   40,
   60,    0,   67,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   64,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   44,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   24,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   30,    0,    0,    0,    0,    0,   26,
   30,   29,   30,   13,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   48,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   24,   21,
  };
  protected static readonly short [] yyCheck = {            44,
    0,    0,   40,   46,   41,   41,   40,   44,   59,   14,
  265,   23,   65,   41,    8,   41,  257,  258,   44,  276,
   73,  257,   59,   35,   18,  257,  265,   40,  257,  123,
  285,   59,  125,  258,  257,   41,   48,   40,    0,   44,
   44,    0,    0,   41,    0,   41,    7,    7,   27,   23,
   47,   -1,   63,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  125,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  125,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  258,   -1,   -1,   -1,   -1,   -1,  262,
  258,  257,  258,  257,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  265,  265,
  };

#line 200 "Parser.jay"


}



#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 Console.Error.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
public class TokenKind {
  public const int IDENTIFIER = 257;
  public const int TYPE_IDENTIFIER = 258;
  public const int INTEGER_LITERAL = 259;
  public const int FLOATING_LITERAL = 260;
  public const int RUNE_LITERAL = 261;
  public const int STRING_LITERAL = 262;
  public const int BREAK = 263;
  public const int DEFAULT = 264;
  public const int FUNC = 265;
  public const int INTERFACE = 266;
  public const int SELECT = 267;
  public const int CASE = 268;
  public const int DEFER = 269;
  public const int GO = 270;
  public const int MAP = 271;
  public const int STRUCT = 272;
  public const int CHAN = 273;
  public const int ELSE = 274;
  public const int GOTO = 275;
  public const int PACKAGE = 276;
  public const int SWITCH = 277;
  public const int CONST = 278;
  public const int FALLTHROUGH = 279;
  public const int IF = 280;
  public const int RANGE = 281;
  public const int TYPE = 282;
  public const int CONTINUE = 283;
  public const int FOR = 284;
  public const int IMPORT = 285;
  public const int RETURN = 286;
  public const int VAR = 287;
  public const int OP_PLUSEQ = 288;
  public const int OP_ELLIPSIS = 289;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }
  internal class yyUnexpectedEof : yyException {
    public yyUnexpectedEof (string message) : base (message) {
    }
    public yyUnexpectedEof () : base ("") {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
