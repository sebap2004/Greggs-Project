using System.Security.Claims;
using Bunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;
using SoftwareProject.Controllers;

namespace UnitTests;

public class AuthenticationControllerShould
{
    private Mock<IAuthenticationService> authServiceMock;
    private Mock<IServiceProvider> serviceProviderMock;
    private Mock<IAccountService> accountServiceMock;
    
    public AuthenticationControllerShould()
    {
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
        accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
    }
    
    [Fact]
    public async Task LoginSubmit_OnSuccess_ShouldReturnSuccessWithAccount()
    {
        //arrange
        var account = new Account
        {
            email = "test@test.com",
            username = "testusername",
            password = "test",
            account_id = 123
        };

        accountServiceMock.Setup(m => m.LoginAccount(account.email, account.password)).ReturnsAsync(account);

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
        var result = await controller.Login(new AccountModel
        {
            Email = account.email,
            Password = account.password
        });
        
        // assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        var objectResult = okResult.Value;
        var type = objectResult.GetType();
        var email = type.GetProperty("Email")?.GetValue(objectResult);
        Assert.Equal(account.email, email);
        var username = type.GetProperty("Username")?.GetValue(objectResult);
        Assert.Equal(account.username, username);
        var id = type.GetProperty("Id")?.GetValue(objectResult);
        Assert.Equal(account.account_id, id);
        var authenticated = (bool?)type.GetProperty("IsAuthenticated")?.GetValue(objectResult);
        Assert.True(authenticated);
        
        accountServiceMock.Verify(m => m.LoginAccount(account.email, account.password), Times.Once);
        authServiceMock.Verify(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()));
    }
    
    [Fact]
    public async Task Register_OnSuccess_ShouldCreateNewAccount()
    {
        //arrange
        var account = new Account
        {
            email = "test@test.com",
            password = "test",
            username = "testName",
        };
        accountServiceMock.Setup(m => m.CreateAccount(It.IsAny<Account>())).ReturnsAsync(RegisterStatus.Success);
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
        var result = await controller.Register(new AccountModel
        {
            Username = account.username,
            Email = account.email,
            Password = account.password
        });
        
        //assert
        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        accountServiceMock.Verify(m => m.CreateAccount(It.Is<Account>(a => a.username == account.username && a.email == account.email)), Times.Once);
        authServiceMock.Verify(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()));
    }
    
    [Fact]
    public async Task  Register_WhenAccountModelIsNull_ShouldReturnBadRequest()
    {
        //arange
        var controller = new AuthenticationController(accountServiceMock.Object);
        
        //act
        var result = await controller.Register(null);
        
        //assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Account data is null", badRequest.Value);
    }
    
    [Theory]
    [InlineData("username", "email", "")]
    [InlineData("", "email", "password")]
    [InlineData("username", "", "password")]
    public async Task  Register_WhenRequiredFieldsAreMissing_ShouldReturnBadRequest(string username, string email, string password)
    {
        //arange
        var controller = new AuthenticationController(accountServiceMock.Object);
        
        //act
        var result = await controller.Register(new AccountModel(){ Username = username, Email = email, Password = password });
        
        //assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Required fields are missing", badRequest.Value);
    }

    [Fact]
    public async Task GetCurrentUser_OnSuccess_ShouldReturnClaims()
    {
        //arrange
        var claims = new List<Claim>() 
        { 
            new(ClaimTypes.Name, "username"),
            new(ClaimTypes.NameIdentifier, "userId"),
            new("name", "Daniel"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        
        var controller = new AuthenticationController(accountServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {
                    RequestServices = serviceProviderMock.Object,
                    User = new ClaimsPrincipal(identity)
                },
            }
        };
        
        //act
        var result = await controller.GetCurrentUser();
      
        //assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
    }
    
    [Fact]
    public async Task Logout_OnSuccess_ShouldReturnOk()
    {
        //arrange
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
        var result = await controller.Logout();
      
        //assert
        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        authServiceMock.Verify(a => a.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()));
    }
}