// Learn more about F# at http://fsharp.org

open System

open Ooui


module GoneUI =

    let textEditor =
        let text = new TextArea (Columns = 72, Rows = 36)
        text.Style.FontFamily <- "Monaco"
        text.Style.FontSize <- 16
        text.Text <- "# This is some code"
        text

    let mainEditor =
        let h = new Heading (1, "TryGo.NET")
        Div (h, textEditor)
















[<EntryPoint>]
let main argv =

    UI.Publish ("/", GoneUI.mainEditor)

    UI.Present ("/")

    while true do
        Threading.Thread.Sleep(1000)

    0 // return an integer exit code
