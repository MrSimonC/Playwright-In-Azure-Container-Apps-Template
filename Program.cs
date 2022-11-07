using Microsoft.Playwright;
using Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseApiKeyAuthentication();
app.MapGet("/health", () => "All OK");
app.MapGet("/", async () => await RunPlaywrightAsync());
app.Urls.Add("http://0.0.0.0:5000");
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

    return "OK";
}