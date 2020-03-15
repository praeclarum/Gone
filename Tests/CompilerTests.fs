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

        func betterThanMain() {
            fmt.Println("Hello, chat room!")
        }
    
        func main() {
    	    fmt.Println("Hello, 世界")
        }""")

    asm.Write ("/Users/fak/Desktop/Output.dll")

    let mainMod = asm.MainModule
    let packageType = mainMod.Types |> Seq.find (fun t -> t.Name = "main")
    Assert.AreEqual (2, packageType.Methods.Count)
