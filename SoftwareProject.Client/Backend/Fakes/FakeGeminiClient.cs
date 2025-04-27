using SoftwareProject.Client.Interfaces;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Backend.Fakes;

/// <summary>
/// Connects to the fake API.
/// Any requests will receive a fixed fake response
/// </summary>
internal class FakeGeminiClient(HttpClient httpClient, string apiKey) : IApiClient
{
    // LOCAL VARIABLES
    // Creates FakeApi class used to call to faked API
    private readonly FakeApi _fakeApi = new();

    /// <summary>
    /// Sends prompt to Fake API and gets its response
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public async Task<string> GeminiCall(string prompt)
    {
        return await _fakeApi.GetResponse(prompt, httpClient, apiKey);
    }
}