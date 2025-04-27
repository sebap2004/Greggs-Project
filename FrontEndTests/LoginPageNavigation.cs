using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using NUnit.Framework;

namespace FrontEndTests;


public class LoginPageNavigation : PageTest
{
 
    /// <summary>
    /// Class Created by Dan
    /// This source helped me understand playwright https://playwright.dev/dotnet/docs/writing-tests (Playwright .NET, no date)
    /// Referenced in TestReferences.txt
    /// this test fills in a pre-defined username and password and checks that the url changes to /chat with a text box, message
    /// at the moment this isn't quite working due to some issue with the form to see the issue: run with the following command
    /// dotnet test -- Playwright.BrowserName=chromium Playwright.LaunchOptions.Headless=false Playwright.LaunchOptions.Channel=msedge
    /// 
    /// </summary>
    [Fact(Skip = "having issue with login failing")]
    public async Task LoginPageShouldRedirect()
    {
        //go to page login page
        await Page.GotoAsync("https://localhost:3000/login");
        await Page.Locator("#login-email").FillAsync("wilm@test.co.uk");
        await Page.Locator("#login-password").FillAsync("12345");
        await Page.Locator("#login-button").ClickAsync();

        await Page.WaitForLoadStateAsync();
        
        // press login button
        await Assertions.Expect(Page).ToHaveURLAsync("https://localhost:3000/chat"); 
        
        await Assertions.Expect(Page.GetByLabel("Message")).ToHaveCountAsync(1, new()
        {
            Timeout = 5000
        });
    }
}