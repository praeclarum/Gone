namespace Gone

open System
open FSharp.Text.Lexing

open Gone.Syntax
open GoParser

type Lexer () =

    static member Tokenize (code : string) : token[] =
        let lexbuf = LexBuffer<char>.FromString code
        let mutable keepGoing = true
        let r = ResizeArray<_> ()
        while keepGoing do
            try
                let tok = Gone.GoLexer.token lexbuf
                r.Add tok
            with _ ->
                keepGoing <- false
        r.ToArray ()



