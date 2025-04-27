using System.Net.Http.Json;
using System.Text.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Client.Clients;

/// <summary>
/// Topic client to be used on the front end. Shares interface with the server Topic Service
/// </summary>
public class TopicClient : ITopicService
{
    /// <summary>
    /// HTTP client to make requests with.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for the client. Injects HTTP client to make requests to the controller.
    /// </summary>
    /// <param name="httpClient">Client to be injected to</param>
    public TopicClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    /// <summary>
    /// Gets topics using HTTP client.
    /// </summary>
    /// <param name="accountId">Account ID to search with.</param>
    /// <returns>List of topics belonging to that account.</returns>
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

    /// <summary>
    /// Deletes a topic using a topic ID as a search index.
    /// </summary>
    /// <param name="topicId">Topic ID to search with</param>
    /// <returns>Deletion status of the topic.</returns>
    public async Task<TopicDeleteStatus> DeleteTopic(int topicId)
    {
        var response = await _httpClient.DeleteAsync($"api/topic/{topicId}");
        if (response.IsSuccessStatusCode)
        {
            return TopicDeleteStatus.Success;
        }

        return TopicDeleteStatus.Failure;
    }

    /// <summary>
    /// Creates a topic. 
    /// </summary>
    /// <param name="topic">Topic to create</param>
    /// <returns>Topic from database</returns>
    public async Task<Topic> CreateTopic(Topic topic)
    {
        Console.WriteLine(JsonSerializer.Serialize(topic));
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