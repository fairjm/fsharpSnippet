///假名翻译为罗马音
#r @"..\packages\Eto.Forms.2.3.0\lib\net45\Eto.dll"
#r @"..\packages\Eto.Platform.Wpf.2.3.0\lib\net45\Eto.Wpf.dll"

open System
open Eto.Forms
open Eto.Drawing
open System.Text

//init map
let str = """
あ い う え お ア イ ウ エ オ
a i u e o a i u e o
か き く け こ カ キ ク ケ コ
ka ki ku ke ko ka ki ku ke ko
さ し す せ そ サ シ ス セ ソ
sa si su se so sa si su se so
た ち つ て と タ チ ツ テ ト
ta ti tu te to ta ti tu te tu
な に ぬ ね の ナ ニ ヌ ネ ノ
na ni nu ne no na ni nu ne no
は ひ ふ へ ほ ハ ヒ フ ヘ ホ
ha hi hu he ho ha hi hu he ho
ま み む め も マ ミ ム メ モ
ma mi mu me mo ma mi mu me mo
や ゆ よ ヤ ユ ヨ
ya yu yo ya yu yo
ら り る れ ろ ラ リ ル レ ロ
ra ri ru re ro ra ri ru re ro
わ を ワ ヲ
wa wo wa wo
ん ン
n n
が ぎ ぐ げ ご
ga gi gu ge go
ざ じ ず ぜ ぞ
za zi zu ze zo
だ ぢ づ で ど
da di du de do
ば び ぶ べ ぼ
pa pi pu pe po
"""

let mapping =
    str.Split([|'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.chunkBySize 2
    |> Seq.map (fun e -> (e.[0].Split([|' '|]), e.[1].Split([|' '|])))
    |> Seq.map (fun (e1, e2) -> Seq.map2 (fun a b -> (a,b)) e1 e2)
    |> Seq.concat
    |> Map.ofSeq

let cells contrs =
    [for c in contrs do
        yield new TableCell(c, true)]

let row (cells: TableCell seq) =
    new TableRow(cells=cells)

let scaleRow (cells: TableCell seq) =
    new TableRow(cells=cells, ScaleHeight = true)

let padding = 5
let translate (input:string) =
    let lines = input.Split([|'\r';'\n'|])
    let getPadding s =
        padding - Encoding.Default.GetByteCount(s + "")
    let trans e =
        let epad = new string(' ', getPadding e) + e
        // two lines.line translated and original line
        match mapping |> Map.tryFind e with
        | Some(r) -> (new string(' ', getPadding r) + r, epad)
        | None -> (epad, epad)
    let r =
        [for line in lines do
            let (t, o) = 
                line.ToCharArray()
                    |> Array.map string
                    |> Array.map trans
                    |> Array.unzip
            yield (String.Concat(t) + "\r\n" + String.Concat(o) + "\r\n")]
    String.Concat(r)

let app = new Application()
let form = new Form(Title="translate to 罗马音", Topmost=true, MinimumSize = new Size(800, 600))

let layout = new TableLayout()
layout.Spacing <- new Size(5, 5)
layout.Padding <- new Padding(10, 10, 10, 10)

let originalText = new TextArea()
let result = new TextArea(ReadOnly = true)

layout.Rows.Add(cells [new Label(Text="原文", Height = 15)] |> row)
layout.Rows.Add(cells [originalText] |> scaleRow)
layout.Rows.Add(cells [new Label(Text="罗马音", Height = 15)] |> row)
layout.Rows.Add(cells [result] |> scaleRow)

form.Content <- layout

originalText.TextChanged.Add (fun _ -> result.Text <- originalText.Text |> translate)
form.Show()
app.Run(form)
