namespace SoftwareProject.Client.Interfaces;

/// <summary>
/// Api Call interface used for api clients to make post requests
/// </summary>
public interface IApiCall
{
    /// <summary>
    /// Gets a response from an AI.
    /// </summary>
    /// <param name="prompt">Prompt to send to AI</param>
    /// <param name="httpClient">Client to make request to an AI API</param>
    /// <param name="apiKey">API key to make the request</param>
    /// <returns></returns>
    Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey);
}