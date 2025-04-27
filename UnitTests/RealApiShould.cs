using RichardSzalay.MockHttp;
using SoftwareProject.Backend;

namespace UnitTests;

public class RealApiShould
{
    /// <summary>
    /// Tests a successful API call will return the expected result. Mocked HTTP is used to simulate a valid reply using Json and fake variables. The code asserts the replies output matches the expected value.
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