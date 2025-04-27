using RichardSzalay.MockHttp;
using SoftwareProject.Backend;

namespace UnitTests;

public class RealApiShould
{
    /// <summary>
    /// Class Created by Dan
    /// Used website as reference point for these tests https://bunit.dev/docs/test-doubles/mocking-httpclient.html (bUnit, no date)
    /// Referenced in TestReferences.txt
    /// Tests a successful API call will return the expected result.
    /// The MockHttp library (MockHttpMessageHandler) is used to simulate a valid reply using Json and fake variables.
    /// The test checks result of the api call matches the expected value.
    /// </summary>
    [Fact]
    public async Task GetResponse_OnSuccess_ReturnsTextFromValidApiResponse()
    {
        //arrange
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
        var api = new RealApi();
        
        // Act
        var result = await api.GetResponse(fakePrompt, httpClient, fakeKey);

        // Assert
        Assert.Equal(fakeResponse, result);
    }
}