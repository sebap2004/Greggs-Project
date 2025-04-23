using SoftwareProject.Backend;
using SoftwareProject.Backend.Fakes;
using FakeItEasy;

namespace SoftwareProject.Services;

public class ApiService
{
    private readonly FakeGeminiClient _fakeGeminiClient;
    private readonly GeminiClient _geminiClient;
    
    public ApiService() {
        string apiKey = "AIzaSyCAZDUnJ_C2YQSJ_YfZVHaJrsTQpwZbqG8";
        var httpClient = new HttpClient();
        _geminiClient = new GeminiClient(httpClient, apiKey);

        var fakeHttpClient = A.Fake<HttpClient>();
        _fakeGeminiClient = new FakeGeminiClient(fakeHttpClient, apiKey);
    }

    public async Task<string> GetMessage(string prompt, bool useFake) 
    {
        Console.WriteLine("Enter prompt");

        string response = "";
        
        if (useFake)
        {
            response = await _fakeGeminiClient.GeminiCall(prompt);
        } else if (!useFake)
        {
            response = await _geminiClient.GeminiCall(prompt);
        }                            

        return response;
    }
}