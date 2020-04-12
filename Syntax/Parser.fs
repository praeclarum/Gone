namespace Gone

open System
open FSharp.Text.Lexing

open Gone.Syntax

type Parser () =

    member this.Parse (code : string) : SourceFile =
        if String.IsNullOrEmpty code then failwith "Cannot parse empty string"
        else
            let lexbuf = LexBuffer<char>.FromString code
            GoParser.source_file GoLexer.token lexbuf



