using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Backend.Fakes;

internal class FakeGeminiClient(HttpClient httpClient, string apiKey) : IApiClient
{
    private readonly FakeApi _fakeApi = new();

    public async Task<string> GeminiCall(string prompt)
    {
        return await _fakeApi.GetResponse(prompt, httpClient, apiKey);
    }
}