using RichardSzalay.MockHttp;
using SoftwareProject.Backend;

namespace UnitTests;

public class RealApiShould
{
    [Fact]
    public async Task GetResponse_OnSuccess__ReturnsTextFromValidApiResponse()
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
            //https://stackoverflow.com/questions/49956455/mock-httpclient-with-multiple-requests
        var httpClient = new HttpClient(mockHttp);
        var api = new RealApi();
        
        // Act
        var result = await api.GetResponse(fakePrompt, httpClient, fakeKey);

        // Assert
        Assert.Equal(fakeResponse, result);
    }
}