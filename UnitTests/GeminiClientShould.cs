using System.Net;
using RichardSzalay.MockHttp;
using SoftwareProject.Client.Backend;

namespace UnitTests;

public class GeminiClientShould
{
    
    /// <summary>
    /// Class Created by Dan
    /// Used website as reference point for these tests https://bunit.dev/docs/test-doubles/mocking-httpclient.html (bUnit, no date)
    /// Referenced in TestReferences.txt
    /// This method is testing that a successful API call made through GeminiClient returns the expected response text. 
    /// The MockHttp library (MockHttpMessageHandler) is used to simulate a valid reply using Json and fake variables.
    /// The test checks result of the api call matches the expected value.
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
    /// This method tests that GeminiClient returns null when the API responds with invalid JSON. 
    /// To do this a mocked response sends broken JSON to verify error handling correctly works.
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
    /// This method tests that TestGeminiConnection returns true on a successful response. 
    /// The MockHttp library (MockHttpMessageHandler) is used to simulate a valid response with status code 200 (OK).
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
    /// This method tests that TestGeminiConnection returns false on an unsuccessful response. 
    /// The MockHttp library (MockHttpMessageHandler) is used to simulates a failed response with status code 500 (Internal Server Error).
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