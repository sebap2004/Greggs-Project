using System.Net.Http.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Client.Clients;

public class TopicClient : ITopicService
{
    private readonly HttpClient _httpClient;

    public TopicClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<Topic>> GetTopics(int accountId)
    {
        var list = await _httpClient.GetFromJsonAsync<List<Topic>>($"api/topic/{accountId}");
        if (list != null)
        {
            return list;
        }

        Console.WriteLine("No topics found for account");
        return new List<Topic>();
    }
    
    public async Task<Topic> CreateTopic(Topic topic)
    {
        var response = await _httpClient.PostAsJsonAsync("api/topic", topic);
        var async = await response.Content.ReadFromJsonAsync<Topic>();
        if (async != null)
        {
            return async;
        }
        Console.WriteLine("Topic creation failed");
        return new Topic();
    }
}