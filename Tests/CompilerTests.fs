module CompilerTests

open Gone.Service
open NUnit.Framework
open System

[<SetUp>]
let Setup () =
    ()

[<Test>]
let IntroToGo () =

    let compiler = Compiler ()
    let code = """
        package main

        import "fmt"

        func betterThanMain() {
            fmt.Println("Hello, chat room!")
        }
    
        func main() {
    	    fmt.Println("Hello, 世界")
        }"""

    let tokens = Gone.Lexer.Tokenize code
    for t in tokens do
        printfn "T %A" t

    let asm = compiler.Compile (code)

    let asmName = "TwoOutput.dll"
    let asmPath = IO.Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), asmName)
    asm.Write asmPath

    let mainMod = asm.MainModule
    let packageType = mainMod.Types |> Seq.find (fun t -> t.Name = "main")
    Assert.AreEqual (2, packageType.Methods.Count)

[<Test>]
let EntryPoint () =

    let compiler = Compiler ()
    let asm = compiler.Compile ("""
        package main

        import "fmt"

        func main() {
            fmt.Println("Hello, 世界")
        }""")

    let asmName = "Output.dll"
    let asmPath = IO.Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), asmName)
    asm.Write asmPath

    let mainMod = asm.MainModule
    let packageType = mainMod.Types |> Seq.find (fun t -> t.Name = "main")
    Assert.AreEqual (1, packageType.Methods.Count)
    Assert.NotNull (mainMod.EntryPoint)
