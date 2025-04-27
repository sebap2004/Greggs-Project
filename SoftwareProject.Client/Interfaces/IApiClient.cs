namespace SoftwareProject.Client.Interfaces;

/// <summary>
/// Api Client interface used for application to call to generic client
/// </summary>
public interface IApiClient
{
    /// <summary>
    /// Makes a call to the gemini AI API
    /// </summary>
    /// <param name="prompt">Prompt to send to the API</param>
    /// <returns>API's Response</returns>
    Task<string> GeminiCall(string prompt);
}