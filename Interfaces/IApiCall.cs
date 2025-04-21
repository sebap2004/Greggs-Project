namespace SoftwareProject.Interfaces;

public interface IApiCall
{
    internal interface IApiCall
    {
        Task<string> GetResponse(string prompt, HttpClient httpClient, string apiKey);
    }
}