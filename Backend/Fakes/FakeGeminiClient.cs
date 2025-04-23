using SoftwareProject.Interfaces;

namespace SoftwareProject.Backend.Fakes;

/// <summary>
/// Connects to the fake API.
/// Any requests will receive an fixed fake response
/// </summary>
internal class FakeGeminiClient(HttpClient httpClient, string apiKey) : IApiClient
{
    // LOCAL VARAIBLES
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