using Microsoft.Playwright;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () => await RunPlaywrightAsync());

app.Run();

async Task<string> RunPlaywrightAsync()
{
    using var playwright = await Playwright.CreateAsync();
    await using var browser = await playwright.Chromium.LaunchAsync();
    var page = await browser.NewPageAsync();
    await page.GotoAsync("https://playwright.dev/dotnet");
    await page.ScreenshotAsync(new()
    {
        Path = "screenshot.png"
    });
    Console.WriteLine("All working.");
    return("All working");
}