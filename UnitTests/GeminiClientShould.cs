using System.Net;
using RichardSzalay.MockHttp;
using SoftwareProject.Backend;

namespace UnitTests;

public class GeminiClientShould
{
    
    /// <summary>
    /// his method is testing that a successful API call made through GeminiClient returns the expected response text. 
    /// Mocked HTTP is used to simulate a valid reply using JSON and fake variables. The code asserts the reply's output matches the expected value.
    /// </summary>
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

    /// <summary>
    /// Tests that GeminiClient returns null when the API call returns a http error. 
    /// To do this a mocked HTTP response is used to simulate the error to verify proper handling.
    /// </summary>
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
    
    /// <summary>
    /// /// This method tests that GeminiClient returns null when the API responds with invalid JSON. 
    /// Mocked response sends broken JSON to verify error handling correctly works.
    /// </summary>
    
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
    
    /// <summary>
    /// This method tests that TestGeminiConnection returns true when the response is done successfully. 
    /// Simulates a valid response with status code 200 ( 200 means OK).
    /// </summary>
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
    
    /// <summary>
    /// This method tests that TestGeminiConnection returns false when the response is done unsuccessfully. 
    /// Simulates a failed response with status code 500 (500 means Internal Server Error).
    /// </summary>
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