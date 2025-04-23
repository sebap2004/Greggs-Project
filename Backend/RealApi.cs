using System.Text;
using System.Text.Json;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Backend;

/// <summary>
/// Formats and posts the prompt and returns Gemini response 
/// </summary>
public class RealApi : IApiClient
{
    /// <summary>
    /// Formats and posts the prompt. Receives and returns response
    /// </summary>
    /// <param name="prompt">Stores the user input to be sent to the API</param>
    /// <param name="httpClient">Client used to connect to the API</param>
    /// <param name="apiKey">Key used to connect to the API</param>
    /// <returns></returns>
    public async Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}");
        // Formats request data
        request.Content = new StringContent(JsonSerializer.Serialize(new
        {
            contents = new[] {
                new {
                    parts = new[] {
                        new { text = prompt }
                    }
                }
            }
        }), Encoding.UTF8, "application/json");
        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        
        // Checks HTTP request was successful
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        // Waits for response, converts from json to string
        var responseContent = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(responseContent);
        var text = json.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

        return text;
    }
}