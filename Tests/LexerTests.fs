module LexerTests

open Gone
open Gone.Parser

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()


[<Test>]
let IdentsAndSomeOps () =

    let code = """
        frank += james
        ()"""

    let tokens = Lexer.Tokenize code

    Assert.AreEqual(5, tokens.Length)
    Assert.AreEqual(Parser.TokenKind.IDENTIFIER, fst tokens.[0])
    Assert.AreEqual("frank", snd tokens.[0])
    Assert.AreEqual(Parser.TokenKind.OP_PLUSEQ, fst tokens.[1])
    Assert.AreEqual(null, snd tokens.[1])


[<Test>]
let LotsOTokens () =

    let code = """
        package main
    
        import "fmt"
    
        func main() {
            fmt.Println("Hello, 世界")
        }"""


    let tokens = Lexer.Tokenize code

    Assert.AreEqual(16, tokens.Length)
    Assert.AreEqual(Parser.TokenKind.STRING_LITERAL, fst tokens.[3])
    Assert.AreEqual("fmt", snd tokens.[3])
    Assert.AreEqual(Parser.TokenKind.PACKAGE, fst tokens.[0])
    Assert.AreEqual("package", snd tokens.[0])
