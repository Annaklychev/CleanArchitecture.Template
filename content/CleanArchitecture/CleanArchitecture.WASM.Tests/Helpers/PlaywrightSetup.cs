using Microsoft.Playwright;

namespace CleanArchitecture.WASM.Tests.Helpers;

public static class PlaywrightSetup
{
    public static async Task<IBrowser> LaunchBrowserAsync(bool headless = true)
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });
        return browser;
    }

    public static async Task<IPage> CreatePageAsync(IBrowser browser, string url)
    {
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        await page.GotoAsync(url);
        return page;
    }
}