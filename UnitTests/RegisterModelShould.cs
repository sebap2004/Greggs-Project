using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SoftwareProject.Components.Pages;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace UnitTests;

public class RegisterModelShould
{
    [Fact]
    public async Task RegisterModel_OnSuccess_ShouldCreateNewAccount()
    {
        var ctx = new TestContext();

        //arrange
        var mockAccountService = new Mock<IAccountService>(MockBehavior.Strict);
        var account = new Account
        {
            email = "test@test.com",
            password = "test",
            username = "testName",
            account_id = 122
        };

        mockAccountService.Setup(m => m.CreateAccount(account)).Returns(Task.CompletedTask);
        var navManager = ctx.Services.GetRequiredService<FakeNavigationManager>();
        var registerModel = new RegisterModel(account, mockAccountService.Object, navManager);

        //act
        var editContext = new EditContext(account);
        await registerModel.RegisterAccount(editContext);

        //assert
        mockAccountService.Verify(m => m.CreateAccount(account), Times.Once);
        Assert.Equal(navManager.BaseUri + "login", navManager.Uri);
    }
}