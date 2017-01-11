/// usage : PhoneNumberLocation phoneNumbers.txt
/// one phone number per line
open System
open System.IO
open System.Net.Http
open FSharp.Data

let queryUrl = "http://sj.apidata.cn/?mobile="

[<Literal>]
let resultSample = """
{
"status": 1,
"data": {
"prefix": 134,
"province": "北京",
"city": "北京",
"isp": "移动",
"code": 10,
"zipcode": 100000,
"types": "中国移动 GSM",
"mobile": "13488888888"
},
"message": "success"
}
"""

type LocationResult = JsonProvider<resultSample>
type PhoneLocation = { Telephone : string; Province : string; City : string; Isp : string}
                     override x.ToString() = sprintf "%s,%s,%s,%s" x.Telephone x.Province x.City x.Isp

let writePhoneLocationAsync (client : HttpClient) (filePath:string) (phone:string) =
    async {
        let api = queryUrl + phone.Trim()
        do printfn "%s" api
        let! data = client.GetStringAsync(api) |> Async.AwaitTask
        let result = LocationResult.Parse(data)
        let r = {Telephone = phone; Province = result.Data.Province; City = result.Data.City; Isp = result.Data.Isp}
        // write directly because of the low speed api
        File.AppendAllLines(filePath, [sprintf "%O" r])
    }

[<EntryPoint>]
let main argv =
    if argv.Length < 1 then
        eprintf "输入文件路径"
        exit(1)
    let filePath = if File.Exists argv.[0] then argv.[0] else Path.Combine(System.Environment.CurrentDirectory ,argv.[0])
    if File.Exists filePath |> not then
        eprintf "文件不存在"
        exit(1)
    
    let resultPath = filePath + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".result.csv"
    printfn "生成文件:%s" resultPath
    if File.Exists resultPath then
        File.Delete resultPath
    File.WriteAllLines(resultPath, ["手机,省,市,运营商"])
    let client = new HttpClient()
    let results = File.ReadLines(filePath)
                    |> Seq.map (fun e -> e.Trim())
                    |> Seq.filter (fun e -> e.Length > 0)
                    |> Seq.map (writePhoneLocationAsync client resultPath)
                    |> Seq.map Async.Catch
                    |> Async.Parallel
                    |> Async.RunSynchronously
    0
