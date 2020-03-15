module Tests

open Gone.Service

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let IntroToGo () =

    let compiler = Compiler ()
    let asm = compiler.Compile ("""
        package main
    
        import "fmt"
    
        func main() {
    	    fmt.Println("Hello, 世界")
        }""")

    let mainMod = asm.MainModule
    let packageType = mainMod.Types |> Seq.find (fun t -> t.Name = "main")
    Assert.AreEqual (1, packageType.Methods.Count)
