using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace FrontEndTests;


public class LoginPageNavigation : PageTest
{
    
    [Fact]
    public async Task LoginPageShouldRedirect()
    {
        //go to page login page
        await Page.GotoAsync("https://localhost:3000/login");
        await Page.Locator("#login-email").FillAsync("wilm@test.co.uk");
        await Page.Locator("#login-password").FillAsync("12345");
        await Page.Locator("[type=submit]").ClickAsync();

        await Page.WaitForLoadStateAsync();
        
        await Assertions.Expect(Page.Locator("#login-password")).ToHaveCountAsync(0, new()
         {
             Timeout = 5000
        });
        
        await Assertions.Expect(Page.GetByLabel("Message")).ToHaveCountAsync(1, new()
        {
            Timeout = 5000
        });
        
   
    }
}