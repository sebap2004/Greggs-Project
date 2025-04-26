using System.Security.Claims;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using SoftwareProject.Client.Models;
using SoftwareProject.Client.Pages;
using SoftwareProject.Controllers;

namespace UnitTests;

public class LoginModelShould
{
    private Mock<IAuthenticationService> authServiceMock;
    private Mock<IServiceProvider> serviceProviderMock;
    private Mock<IAccountService> accountServiceMock;
    private Account account;
    
    public LoginModelShould()
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
        accountServiceMock.Setup(m => m.LoginAccount(account.email, account.password)).ReturnsAsync(account);
    }
    
    [Fact]
    public async Task LoginSubmit_OnSuccess_ShouldNavigateToBaseUrl()
    {
        var ctx = new TestContext();
        //arrange
        var navManager = ctx.Services.GetRequiredService<FakeNavigationManager>();
        // var loginModel = new LoginModel(account, httpClient);
        var controller = new AuthenticationController(accountServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {
                    RequestServices = serviceProviderMock.Object
                }
            }
        };
        //act
        // await loginModel.LoginSubmit();
        await controller.Login(account.AccountModel);;
        //assert
        accountServiceMock.Verify(m => m.LoginAccount(account.email, account.password), Times.Once);
        Assert.Equal(navManager.BaseUri, navManager.Uri);
    }
}