using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace FrontEndTests;


public class LoginPageNavigation
{
    
    [Fact]
    public async Task LoginPageShouldRedirect()
    {
        //Initialise Playwright
        var playwright = await Playwright.CreateAsync();
        //'Chromium' Firefox' 'Webkit'
        var browser = await playwright
            .Chromium
            .LaunchAsync();

        var browserContext = await browser.NewContextAsync();
        var page = await browserContext.NewPageAsync();
        
        //navmanager
        var ctx = new TestContext();
        var navManager = ctx.Services.GetRequiredService<FakeNavigationManager>();

        //go to page
        await page.GotoAsync("https://localhost:3000/login");
        await page.ScreenshotAsync(new PageScreenshotOptions {Path = "login.png"});
        // await page.GetByRole(AriaRole.Link, new(){Name = "LOG IN"}).ClickAsync();
        await page.Locator("text=Sign In").ClickAsync();
        await page.Locator("#email").FillAsync("wilm@test.co.uk");
        await page.Locator("#password").FillAsync("12345");
        await page.Locator("[type=submit]").ClickAsync();
        
        await Assertions.Expect(page.Locator("h1")).ToHaveTextAsync("Welcome to NovaChat", new LocatorAssertionsToHaveTextOptions
        {
            Timeout = 5000 //  Increased timeout to 5 seconds (5000ms) to handle Blazor load times
        });        
        
    }
}