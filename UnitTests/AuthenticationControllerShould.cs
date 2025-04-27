using System.Security.Claims;
using Bunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
    
    /// <summary>
    /// Class Created by Dan
    /// The Authentication Controller had several methods that were awkward to mock, (HttpContext.SignAsync and SignOutAsync
    /// in ControllerBase). To mock these methods, it was necessary to initialise the Controller's ControllerContext with a custom 
    /// HttpContext, where RequestServices was replaced with a mock service provider (serviceProviderMock). This mock service provider 
    /// returns the authentication service mock, which contains the mocked implementations of SignInAsync and SignOutAsync.
    /// got the idea from https://stackoverflow.com/questions/47198341/how-to-unit-test-httpcontext-signinasync (HeroWong, 2021)
    /// </summary>
    public AuthenticationControllerShould()
    {
        authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock
            .Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);
        serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);        
        accountServiceMock = new Mock<IAccountService>(MockBehavior.Strict);
    }
    
    /// <summary>
    /// Initialisation of controller adapted from (HeroWong, 2021).
    /// Tests calling lLoginAccount on the AuthenticationController with a valid username and password.
    /// Verifies that the correct methods — SignInAsync and LoginAccount (mocked services) — are called
    /// and that a successful result is returned with the expected object and properties.
    /// </summary>
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
        }; //(HeroWong, 2021)
        
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
    
    /// <summary>
    /// Initialisation of controller adapted from (HeroWong, 2021).
    /// When the Register method is called on the AccountController with valid data.
    /// Verifies that the correct methods — SignInAsync and CreateAccount (mocked services) — are called
    /// and that a successful result is returned with the expected object and properties.
    /// </summary>
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
        accountServiceMock.Setup(m => m.CreateAccount(It.IsAny<Account>())).ReturnsAsync(new CreateAccountResultDto {status = RegisterStatus.Success});
        var controller = new AuthenticationController(accountServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {
                    RequestServices = serviceProviderMock.Object
                }
            }
        }; //(HeroWong, 2021)
        
        //act
        var result = await controller.Register(new AccountModel
        {
            Username = account.username,
            Email = account.email,
            Password = account.password
        });
        
        //assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        accountServiceMock.Verify(m => m.CreateAccount(It.Is<Account>(a => a.username == account.username && a.email == account.email)), Times.Once);
        authServiceMock.Verify(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()));
    }
    
    /// <summary>
    /// When the Register method is called on tge AccountController with invalid data,
    /// checks a bad request is returned.
    /// </summary>
    [Fact]
    public async Task  Register_WhenAccountModelIsNull_ShouldReturnBadRequest()
    {
        //arrange
        var controller = new AuthenticationController(accountServiceMock.Object);
        
        //act
        var result = await controller.Register(null);
        
        //assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Account data is null", badRequest.Value);
    }
    
    ///<summary>
    /// When the Register method is called on tge AccountController with empty fields
    /// checks a bad request is returned.
    /// </summary>
    [Theory]
    [InlineData("username", "email", "")]
    [InlineData("", "email", "password")]
    [InlineData("username", "", "password")]
    public async Task  Register_WhenRequiredFieldsAreMissing_ShouldReturnBadRequest(string username, string email, string password)
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
        var result = await controller.Register(new AccountModel(){ Username = username, Email = email, Password = password });
        //assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Required fields are missing", badRequest.Value);
    }
    
    /// <summary>
    /// Initialisation of controller adapted from (HeroWong, 2021).
    /// When the GetCurrentUser method is called on the AccountController, 
    /// checks that success is returned (200) with a list of claims. 
    /// </summary>
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
        }; //(HeroWong, 2021). 
        
        //act
        var result = await controller.GetCurrentUser();
      
        //assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
    }
    
    /// <summary>
    /// Initialisation of controller adapted from (HeroWong, 2021).
    /// When logout out method is called
    /// Verifies that the correct methods — SignOutAsync (mocked services) — are called
    /// Check that status is ok (200) 
    /// </summary>
    [Fact]
    public async Task Logout_OnSuccess_ShouldReturnOk()
    {
        //arrange
        authServiceMock
            .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);
        var controller = new AuthenticationController(accountServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {
                    RequestServices = serviceProviderMock.Object
                }
            }
        }; //(HeroWong, 2021)
        
        //act
        var result = await controller.Logout();
      
        //assert
        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        authServiceMock.Verify(a => a.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()));
    }
}