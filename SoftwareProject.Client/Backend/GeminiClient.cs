using SoftwareProject.Interfaces;
using System.Text;
using System.Text.Json;

namespace SoftwareProject.Backend;

public class GeminiClient(HttpClient httpClient, string apiKey) : IApiClient
{
    private readonly RealApi _realApi = new();

    public async Task<string?> GeminiCall(string prompt)
    {
        try
        {
            return await _realApi.GetResponse(prompt, httpClient, apiKey);
        } catch (HttpRequestException ex)
        {
            Console.WriteLine($"API Request Error: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            return null;
        }
    }

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