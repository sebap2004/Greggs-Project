using System.Security.Claims;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SoftwareProject.Client.Pages;
using SoftwareProject.Controllers;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;

namespace UnitTests;

public class RegisterModelShould
{
    private Mock<IAuthenticationService> authServiceMock;
    private Mock<IServiceProvider> serviceProviderMock;
    private Mock<IAccountService> accountServiceMock;
    private Account account;
    
    public RegisterModelShould()
    {
        account = new Account
        {
            username = "testusername",
            account_id = 123,
            email = "test@test.com",
            password = "test"
        };
        authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock
            .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);
        authServiceMock
            .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);
        serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(_ => _.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);        
        accountServiceMock = new Mock<IAccountService>(MockBehavior.Loose);
        accountServiceMock.Setup(m => m.CreateAccount(account)).ReturnsAsync(new CreateAccountResultDto
        {
            status = RegisterStatus.Success,
            accountId = 0,
        });
    }
    
    [Fact]
    public async Task RegisterModel_OnSuccess_ShouldCreateNewAccount()
    {
        var ctx = new TestContext();

        //arrange
        var navManager = ctx.Services.GetRequiredService<FakeNavigationManager>();
        var controller = new AuthenticationController(accountServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {
                    RequestServices = serviceProviderMock.Object
                }
            }
        };
        await controller.Register(account.AccountModel);
        //assert
        accountServiceMock.Verify(m => m.CreateAccount(It.IsAny<Account>()), Times.Once);
            Assert.Equal(navManager.BaseUri, navManager.Uri);
    }
}