// Learn more about F# at http://fsharp.org

open System

open Ooui


module GoneUIData =

    type Data =
        {
            FilePath : string option
            SourceCode : string
            Error : string
            Ast : string
            DecompiledCode : string
        }

    type DataStore() =
        let updated = Event<_> ()
        let mutable data =
            {
                FilePath = None
                SourceCode = ""
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


module Services =

    open GoneUIData

    type AstService (data : DataStore) =
        let updateAst () : unit =
            data.Update (fun d ->
                try
                    let parser = Gone.Parser.GoParser ()
                    let ast = parser.Parse d.SourceCode
                    let stringAst = sprintf "%A" ast
                    { d with Ast = stringAst; Error = "" }
                with ex ->
                    { d with Ast = ""; Error = string ex })

        do
            data.Updated.Add updateAst
            updateAst ()

    type SaveService (data : DataStore) =
        let saveFile () : unit =
            let d = data.Data
            match d.FilePath with
            | None -> ()
            | Some path ->
                IO.File.WriteAllText (path, d.SourceCode)

        do data.Updated.Add saveFile

module GoneUI =

    open GoneUIData
    open Services

    let data = GoneUIData.DataStore ()

    let astService = AstService (data)
    let saveService = SaveService (data)

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
        let text = new Div ()
        text.Style.["white-space"] <- "pre"
        text.Style.FontFamily <- "Monaco"
        text.Style.FontSize <- 12
        text.Text <- data.Data.Ast
        data.Updated.Add (fun () ->
            text.Text <- data.Data.Ast)
        text

    let mainEditor fileToOpen =
        let initialCode =
            match fileToOpen with
            | None -> "# No Code!"
            | Some path ->
                try IO.File.ReadAllText path
                with _ -> "# Failed to read code"
        data.Update (fun d ->
            { d with SourceCode = initialCode
                     FilePath = fileToOpen })

        let h = new Heading (1, "TryGo.NET")
        Div (h, textEditor, errorDisplay, astDisplay)














open System

[<EntryPoint>]
let main argv =


    Console.WriteLine ("HI Y'ALL 1")

    //for a in argv do
        //Console.WriteLine ("COMMAND LINE: {0}", a)

    let fileToOpen =
        if argv.Length > 0 && argv.[0].EndsWith (".go", StringComparison.InvariantCultureIgnoreCase) then
            Some argv.[0]
        else None

    Console.WriteLine ("HI Y'ALL 2")

    UI.Publish ("/", GoneUI.mainEditor fileToOpen)

    Console.WriteLine ("HI Y'ALL 3")

    //UI.Present ("/")

    Console.WriteLine ("HI Y'ALL 4")

    //while true do
        //Threading.Thread.Sleep(1000)

    0 // return an integer exit code
