using Bunit;
using Bunit.TestDoubles;
using Moq;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SoftwareProject.Client.Pages;

namespace UnitTests;

public class LoginModelShould
{
    [Fact]
    public async Task LoginSubmit_OnSuccess_ShouldNavigateToBaseUrl()
    {
        var ctx = new TestContext();
        
        //arrange
        var mockAccountService = new Mock<IAccountService>(MockBehavior.Strict);
        var account = new Account
        {
            email = "test@test.com",
            password = "test"
        };

        mockAccountService.Setup(m => m.LoginAccount(account.email, account.password)).ReturnsAsync(new Account());
        var navManager = ctx.Services.GetRequiredService<FakeNavigationManager>();
        var loginModel = new LoginModel(account, mockAccountService.Object, navManager);
        
        //act
        await loginModel.LoginSubmit();

        //assert
        mockAccountService.Verify(m => m.LoginAccount(account.email, account.password), Times.Once);
        Assert.Equal(navManager.BaseUri, navManager.Uri);
    }
}