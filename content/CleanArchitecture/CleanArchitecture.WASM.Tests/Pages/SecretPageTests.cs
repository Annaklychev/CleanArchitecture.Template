using CleanArchitecture.WASM.Tests.Helpers;
using FluentAssertions;

namespace CleanArchitecture.WASM.Tests.Pages;

public class SecretPageTests
{
    private const string BaseUrl = "https://localhost:7175";

    [Fact]
    public async Task SecretPage_ShouldShowNotAuthorized_WhenNotAuthenticated()
    {
        // Launch the browser
        var browser = await PlaywrightSetup.LaunchBrowserAsync();
        var page = await PlaywrightSetup.CreatePageAsync(browser, $"{BaseUrl}/SecretPage");

        // Find the <p> element with the text Not authorized
        var notAuthorizedText = page.Locator("article.content p");

        (await notAuthorizedText.InnerTextAsync()).Should().Be("Not authorized");

        await browser.CloseAsync();
    }
}