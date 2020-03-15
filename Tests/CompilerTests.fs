module Tests

open Gone.Service
open NUnit.Framework
open System

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

    let asmPath = IO.Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output.dll")
    asm.Write asmPath

    let mainMod = asm.MainModule
    let packageType = mainMod.Types |> Seq.find (fun t -> t.Name = "main")
    Assert.AreEqual (2, packageType.Methods.Count)
