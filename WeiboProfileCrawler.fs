//nuget webdriver and canopy
#r @"..\packages\Selenium.WebDriver.3.0.0\lib\net40\WebDriver.dll"
#r @"..\packages\canopy.1.0.6\lib\canopy.dll"

open canopy
open System
open runner
open OpenQA.Selenium
open System.IO
open System.Threading

//html output dir
let outDir = @"D:\weibo\"

let writePage pageNum content = 
    let outFile = outDir + (sprintf "%s_%d.html" (DateTime.Now.ToString("yyyy_MM_dd")) pageNum)
    File.WriteAllText(outFile, content)

// wait time set to 60sec
compareTimeout <- 60.0

let pressEnd () =
    press Keys.End

let getHtml () =
    let ele = element "html"
    ele.GetAttribute("outerHTML")

let getNextPageElement () =
    try
        Some(element "a.page.next")
    with
        _ -> None

// dir of chromedriver.exe
chromeDir <- @"D:\webdriver\chrome"

// new chrome instance
start chrome

// goto the url
// enter username password and login
url "https://weibo.com"

printfn "login~"

// wait to go to profile page
waitFor (fun () -> 
            let now = currentUrl()
            now.Contains("/profile"))

printfn "go on"

let random = new Random()

let threadSleep (millis:int) =
    Thread.Sleep(millis)

let pressToProfileEnd () =
    let pressIt() =
        pressEnd()
        sleep 4
        pressEnd()
        sleep 4
        pressEnd()
        sleep 4
    pressIt()
    // try it again
    let notHasNext = getNextPageElement().IsNone
    if notHasNext then
        reload()
        pressIt()

let rec handleUserPage pageNum =
    // scoll to the end
    pressToProfileEnd ()
    let content = getHtml()
    writePage pageNum content
    match getNextPageElement() with
    | Some(e) ->
        // don't do it too quick
        random.Next(500) + 500 |> threadSleep
        // let's go to next page
        click e
        handleUserPage (pageNum + 1)
    | None ->
        printfn "it seems over"

handleUserPage 0
