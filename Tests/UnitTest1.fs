module Tests

open Gone

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let IntroToGo () =

    let compiler = Compiler ()
    compiler.Compile ("""
        package main
    
        import "fmt"
    
        func main() {
    	    fmt.Println("Hello, 世界")
        }""")

    Assert.Pass()
