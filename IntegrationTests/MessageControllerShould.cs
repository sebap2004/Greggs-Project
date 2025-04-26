using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SoftwareProject.Client.Data;

namespace IntegrationTests;

public class MessageControllerShould
{
    private readonly string baseUrl = "api/message";
    
    [Fact]
    public async Task PostAsync_WithValidMessage_ShouldReturnSuccess()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        // Act
        const int topicId = 1; // assumes a topic of 1 always exists
        var messageDto = new MessageDto() {AiResponse = true, MessageText = "Hello World", TopicId = topicId};
        var content = JsonContent.Create(messageDto);
        
        var response = await client.PostAsync(baseUrl, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAsync_WithValidTopic_ShouldReturnListOfMessages()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        // Act
        const int topicId = 1; // assumes a topic of 1 always exists
        var response = await client.GetAsync(baseUrl + '/' + topicId);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseBody);
    }
}