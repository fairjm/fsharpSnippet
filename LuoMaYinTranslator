///假名翻译为罗马音
#r @"..\packages\Eto.Forms.2.3.0\lib\net45\Eto.dll"
#r @"..\packages\Eto.Platform.Wpf.2.3.0\lib\net45\Eto.Wpf.dll"

open Eto.Forms
open Eto.Drawing
 
let app = new Application()
let form = new Form(Title="translate to 罗马音", Topmost=true, MinimumSize = new Size(800, 600))

let cells contrs =
    [for c in contrs do
        yield new TableCell(c, true)]

let row (cells: TableCell seq) =
    new TableRow(cells=cells)

let scaleRow (cells: TableCell seq) =
    new TableRow(cells=cells, ScaleHeight = true)

let layout = new TableLayout()
layout.Spacing <- new Size(5, 5)
layout.Padding <- new Padding(10, 10, 10, 10)

let originalText = new TextArea()
originalText.TextChanged.Add (fun _ -> printfn "text changed %s" originalText.Text)

let result = new TextArea(ReadOnly = true)

layout.Rows.Add(cells [new Label(Text="原文", Height = 15)] |> row)
layout.Rows.Add(cells [originalText] |> scaleRow)
layout.Rows.Add(cells [new Label(Text="罗马音", Height = 15)] |> row)
layout.Rows.Add(cells [result] |> scaleRow)

form.Content <- layout
form.Show()
app.Run(form)
