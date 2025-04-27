namespace SoftwareProject.Client.Interfaces;

/// <summary>
/// Api Call interface used for api clients to make post requests
/// </summary>
public interface IApiCall
{
    internal interface IApiCall
    {
        Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey);
    }
}