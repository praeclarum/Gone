namespace Gone



type Compiler () =
    let x = 42

    member this.Compile (code : string) =

        let parser = Parser.GoParser ()

        let translationUnit = parser.Parse code

        ()

