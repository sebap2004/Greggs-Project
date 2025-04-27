using System.Net.Http.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Client.Clients;


/// <summary>
/// Message client to be used on the frontend. Shares the same interface as the Message Service on the Server.
/// </summary>
public class MessageClient : IMessageService
{
    /// <summary>
    /// HTTP client to make requests with.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for client. Injects HTTP client for making HTTP requests.
    /// </summary>
    /// <param name="httpClient">HTTP Client to be injected</param>
    public MessageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    
    /// <summary>
    /// Gets the messages using the HTTP client, which calls to the API controller on the server.
    /// </summary>
    /// <param name="topicId">Topic ID to search with</param>
    /// <returns>List of message data transfer objects.</returns>
    public async Task<List<MessageDto>> GetMessages(int topicId)
    {
        List<MessageDto>? messages = await _httpClient.GetFromJsonAsync<List<MessageDto>>($"api/message/{topicId}");
        if (messages == null)
        {
            Console.WriteLine("No messages found for topic " + topicId);
            return new List<MessageDto>();
        }
        return messages;
    }

    /// <summary>
    /// Creates messages using the HTTP client, which calls to the API controller on the server.
    /// </summary>
    /// <param name="message">Message data transfer object to create.</param>
    /// <returns>Status of the created message.</returns>
    public async Task<MessageSendStatus> CreateMessage(MessageDto message)
    {
        var sendAttempt = await _httpClient.PostAsJsonAsync("api/message", message);
        if (sendAttempt.IsSuccessStatusCode)
        {
            return MessageSendStatus.Success;
        }
        return MessageSendStatus.Failure;
    }
}