// Learn more about F# at http://fsharp.org

open System

open Ooui


module GoneUIData =

    type Data =
        {
            SourceCode : string
            Error : string
            Ast : string
            DecompiledCode : string
        }

    type DataStore() =
        let updated = Event<_> ()
        let mutable data =
            {
                SourceCode = "# Start typing here"
                Error = ""
                Ast = ""
                DecompiledCode = ""
            }
        member this.Update (update : Data -> Data) =
            let newData = update data
            if newData <> data then
                data <- newData
                updated.Trigger ()
        member this.Data = data
        member this.Updated = updated.Publish

        member this.UpdateSourceCode (code) =
            this.Update (fun d -> {
                d with SourceCode = code })


module GoneUI =

    open GoneUIData

    let data = GoneUIData.DataStore ()

    let astService =
        let updateAst () : unit =
            data.Update (fun d ->
                try
                    let parser = Gone.Parser.GoParser ()
                    let ast = parser.Parse d.SourceCode
                    let stringAst = sprintf "%A" ast
                    { d with Ast = stringAst; Error = "" }
                with ex ->
                    { d with Ast = ""; Error = string ex })

        data.Updated.Add updateAst
        updateAst ()
        ()

    let textEditor =
        let text = new TextArea (Columns = 72, Rows = 12)
        text.Style.FontFamily <- "Monaco"
        text.Style.FontSize <- 16
        text.Text <- data.Data.SourceCode
        let updateUI () =
            text.Value <- data.Data.SourceCode
        data.Updated.Add updateUI
        updateUI ()
        text.Input.Add (fun _ ->
            data.UpdateSourceCode (text.Value)
            ())
        text

    let errorDisplay =
        let text = new Paragraph ()
        text.Style.FontFamily <- "sans-serif"
        text.Style.FontSize <- 14
        text.Style.Color <- "#800"
        text.Text <- data.Data.Error
        let updateUI () =
            text.Text <- data.Data.Error
        data.Updated.Add updateUI
        updateUI ()
        text

    let astDisplay =
        let text = new TextArea (Columns = 72, Rows = 36)
        text.Style.FontFamily <- "Monaco"
        text.Style.FontSize <- 16
        text.Text <- data.Data.Ast
        data.Updated.Add (fun () ->
            text.Text <- data.Data.Ast)
        text

    let mainEditor =
        let h = new Heading (1, "TryGo.NET")
        Div (h, textEditor, errorDisplay, astDisplay)
















[<EntryPoint>]
let main argv =

    UI.Publish ("/", GoneUI.mainEditor)

    UI.Present ("/")

    while true do
        Threading.Thread.Sleep(1000)

    0 // return an integer exit code
