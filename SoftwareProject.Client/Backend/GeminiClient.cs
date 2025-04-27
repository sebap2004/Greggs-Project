using System.Text;
using System.Text.Json;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Client.Backend;

/// <summary>
/// Connects to the Gemini API.
/// Used to communicate requests and receive responses
/// </summary>
public class GeminiClient(HttpClient httpClient, string apiKey) : IApiClient
{
    // LOCAL VARIABLES
    // Creates RealApi class used to call to Gemini
    private readonly RealApi _realApi = new();

    /// <summary>
    /// Sends prompt to Gemini API and gets it response
    /// Catches exceptions
    /// </summary>
    /// <param name="prompt">Stores the user input to be sent to the API</param>
    /// <returns></returns>
    public async Task<string> GeminiCall(string prompt)
    {
        try
        {
            return await _realApi.GetResponse(prompt, httpClient, apiKey);
        } catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON parse error: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Tests connection status with the Gemini API using the key
    /// </summary>
    /// <returns></returns>
    public async Task<bool> TestGeminiConnection()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}");
            request.Content = new StringContent("{\"contents\": [{\"parts\": [{\"text\": \"test\"}]}]}", Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}