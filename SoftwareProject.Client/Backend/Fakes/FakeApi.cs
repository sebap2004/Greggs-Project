using System.Net;
using System.Text;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Backend.Fakes;

/// <summary>
/// Returns a fake API response from an input
/// </summary>
public class FakeApi : IApiCall
{
    /// <summary>
    /// Sends a post request to the API but is intercepted by a fake API message
    /// </summary>
    /// <param name="prompt">Not used. Stores the user input to be sent to the API</param>
    /// <param name="httpClient">Client used to connect to the API</param>
    /// <param name="apiKey">Not used. Key used to connect to the API</param>
    /// <returns></returns>
    public async Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey)
    {
        string customResponse = "Hi. I am a fake response.";
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(customResponse, Encoding.UTF8, "application/json")
        };
        
        var fakeHttpClient = new HttpClient(new FakeHttpMessageHandler(fakeResponse));
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}");
        
        HttpResponseMessage response = await fakeHttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}