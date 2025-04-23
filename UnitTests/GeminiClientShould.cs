using System.Net;
using RichardSzalay.MockHttp;
using SoftwareProject.Backend;

namespace UnitTests;

public class GeminiClientShould
{
    [Fact]
    public async Task GeminiCall_ReturnsCorrectText_FromValidApiResponse()
    {
        // Arrange
        var fakeResponse = "Hello from the API!";
        var fakePrompt = "Say hello";
        var fakeKey = "fake-key";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={fakeKey}")
            .Respond("application/json", $@"{{
                    ""candidates"": [
                        {{
                            ""content"": {{
                                ""parts"": [
                                    {{ ""text"": ""{fakeResponse}"" }}
                                ]
                            }}
                        }}
                    ]
                }}");

        var httpClient = new HttpClient(mockHttp);
        
        var geminiClient = new GeminiClient(httpClient, fakeKey);

        // Act
        var result = await geminiClient.GeminiCall(fakePrompt);

        // Assert
        Assert.Equal(fakeResponse, result);
    }

    [Fact]
    public async Task GeminiCall_ReturnsNull_FromHttpError()
    {      
        var fakeResponse = "Hello from the API!";
        var fakePrompt = "Say hello";
        var fakeKey = "fake-key";
        var mockHttp = new MockHttpMessageHandler();
        //h2 import lib 
        mockHttp.When(HttpMethod.Post, $"brokenLink").Respond(HttpStatusCode.InternalServerError);
        
        var httpClient = new HttpClient(mockHttp);
        
        var geminiClient = new GeminiClient(httpClient, fakeKey);

        // Act
        var result = await geminiClient.GeminiCall(fakePrompt);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GeminiCall_ReturnsNull_FromJsonError()
    {
        // Arrange
        var fakePrompt = "Say hello";
        var fakeKey = "fake-key";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={fakeKey}")
            .Respond("application/json", @"{ ""Json Broken""}");
                 

        var httpClient = new HttpClient(mockHttp);
        
        var geminiClient = new GeminiClient(httpClient, fakeKey);

        // Act
        var result = await geminiClient.GeminiCall(fakePrompt);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task TestGeminiConnection_ReturnsTrue_UponSuccess()
    {
        // Arrange
        var fakeKey = "fake-key";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={fakeKey}")
            .Respond(HttpStatusCode.OK);
                 

        var httpClient = new HttpClient(mockHttp);
        
        var geminiClient = new GeminiClient(httpClient, fakeKey);

        // Act
        var result = await geminiClient.TestGeminiConnection();

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task TestGeminiConnection_ReturnsFalse_UponFailure()
    {
        // Arrange
        var fakeKey = "fake-key";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={fakeKey}")
            .Respond(HttpStatusCode.InternalServerError);
                 

        var httpClient = new HttpClient(mockHttp);
        
        var geminiClient = new GeminiClient(httpClient, fakeKey);

        // Act
        var result = await geminiClient.TestGeminiConnection();

        // Assert
        Assert.False(result);
    }
}