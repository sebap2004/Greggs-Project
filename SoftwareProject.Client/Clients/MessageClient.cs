using System.Net.Http.Json;
using SoftwareProject.Client.Data;
using SoftwareProject.Client.Interfaces;

namespace SoftwareProject.Client.Clients;

public class MessageClient : IMessageService
{
    private readonly HttpClient _httpClient;

    public MessageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
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