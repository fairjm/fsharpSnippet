// 微博个人页爬取
// nuget webdriver and canopy
#r @"..\packages\Selenium.WebDriver.3.0.0\lib\net40\WebDriver.dll"
#r @"..\packages\canopy.1.0.6\lib\canopy.dll"

open canopy
open System
open runner
open OpenQA.Selenium

let outDir = @"D:\weibo\"

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

// get chromedriver.exe ready
chromeDir <- @"D:\webdriver\chrome"

// new chrome instance
start chrome

// goto the url
// enter username password and login
url "https://weibo.com"


// wait to go to profile page
waitFor (fun () -> 
            let now = currentUrl()
            now.Contains("/profile"))

printfn "go on"
