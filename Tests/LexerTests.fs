module LexerTests

open Gone

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let LotsOTokens () =

    let code = """
        package main
    
        import "fmt"
    
        func main() {
            fmt.Println("Hello, 世界")
        }"""


    let parser = Parser.GoParser ()
    let ast = parser.Parse (code)

    Assert.Pass()
