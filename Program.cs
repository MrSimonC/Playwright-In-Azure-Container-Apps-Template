using Microsoft.Playwright;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", async () => await RunPlaywrightAsync());
app.Run();

async Task<string> RunPlaywrightAsync()
{
    using var playwright = await Playwright.CreateAsync();
    var chromium = playwright.Chromium;
    var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { 
        Channel = "msedge",
        ExecutablePath = "/usr/bin/microsoft-edge-stable",
        ChromiumSandbox = false
    });
    var context = await browser.NewContextAsync(new BrowserNewContextOptions{
        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.28"
    });
    var page = await context.NewPageAsync();
    await page.GotoAsync("https://www.whatsmybrowser.org");
    await page.ScreenshotAsync(new()
    {
        Path = "screenshot.png",
        FullPage = true
    });
    return(await page.Locator(".header").First.TextContentAsync() ?? "Element not found simon");
}