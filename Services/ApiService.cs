using SoftwareProject.Backend;
using SoftwareProject.Backend.Fakes;
using FakeItEasy;

namespace SoftwareProject.Services;

/// <summary>
/// ApiService takes inputs and sends them as requests to the real Gemini API or Fake API
/// Use to communicate with the chatbot
/// </summary>
public class ApiService
{
    // CLASS Variables
    // Assign Fake Gemini Client
    private readonly FakeGeminiClient _fakeGeminiClient;
    // Assign Real Gemini Client
    private readonly GeminiClient _geminiClient;
    
    /// <summary>
    /// CONSTRUCTOR
    /// Creates Real&Fake Http clients and Gemini clients
    /// </summary>
    public ApiService() {
        // Backup apiKey: AIzaSyBBf3wxLwAe_OLbunkfhMQTIZMTfmq5e2M
        string apiKey = "AIzaSyCAZDUnJ_C2YQSJ_YfZVHaJrsTQpwZbqG8";
        var httpClient = new HttpClient();
        _geminiClient = new GeminiClient(httpClient, apiKey);

        var fakeHttpClient = A.Fake<HttpClient>();
        _fakeGeminiClient = new FakeGeminiClient(fakeHttpClient, apiKey);
    }

    /// <summary>
    /// Gets prompt input and returns a response from the real/stub API
    /// </summary>
    /// <param name="prompt">Stores the user input to be sent to the API</param>
    /// <param name="useFake">Toggle decides to use the real or stub API</param>
    /// <returns></returns>
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