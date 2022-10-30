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
    var page = await browser.NewPageAsync();
    await page.GotoAsync("https://playwright.dev/dotnet");
    await page.ScreenshotAsync(new()
    {
        Path = "screenshot.png"
    });
    return("All working.");
}