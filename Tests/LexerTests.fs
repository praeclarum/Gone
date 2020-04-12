module LexerTests

open Gone
open Gone.GoParser

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

    Assert.AreEqual(6, tokens.Length)
    Assert.IsTrue(match tokens.[0] with IDENTIFIER _ -> true | _ -> false)
    Assert.AreEqual("frank", match tokens.[0] with IDENTIFIER x -> x | _ -> "")
    Assert.IsTrue(match tokens.[1] with OP_PLUSEQ _ -> true | _ -> false)
    Assert.IsTrue(match tokens.[5] with EOF -> true | _ -> false)

[<Test>]
let LotsOTokens () =

    let code = """
        package main

        import "fmt"
        func main() {
            fmt.Println("Hello, 世界")
        }"""


    let tokens = Lexer.Tokenize code

    Assert.AreEqual(17, tokens.Length)
