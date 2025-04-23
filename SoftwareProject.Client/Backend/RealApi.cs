using System.Text;
using System.Text.Json;
using SoftwareProject.Interfaces;

namespace SoftwareProject.Backend;

public class RealApi : IApiClient
{
    public async Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey)
    {
        Console.WriteLine("Prompt getting added: " + prompt);
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}");
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

        string result = await request.Content.ReadAsStringAsync();
        Console.WriteLine(result);
        
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(responseContent);
        var text = json.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

        return text;
    }
}