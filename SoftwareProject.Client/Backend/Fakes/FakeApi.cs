using System.Net;
using System.Text;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Backend.Fakes;

public class FakeApi : IApiCall
{
    public async Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey)
    {
        string customResponse = "Hi. I am a fake response.";

        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(customResponse, Encoding.UTF8, "application/json")
        };

        var fakeHttpClient = new HttpClient(new FakeHttpMessageHandler(fakeResponse));
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}");

        HttpResponseMessage response = await fakeHttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}