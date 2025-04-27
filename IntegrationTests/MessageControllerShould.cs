using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SoftwareProject.Client.Data;

namespace IntegrationTests;

/// <summary>
/// see https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0 (Microsoft, no date) Code wasn't directly copied but was used for inspiration
/// Referenced in TestReferences.txt
/// created a new webApplication factory (uses factory pattern)
/// </summary>
public class MessageControllerShould
{
    private readonly string baseUrl = "api/message";
    
    /// <summary>
    /// The WebApplicationFactory is used to create a test server instance of our application (Program).
    /// A sample json message is then created using JsonContent.Create.
    /// The PostAsync method is invoked with the json message.
    /// The test then checks to see that the server responded with ok.
    /// </summary>
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
    
    /// <summary>
    /// The WebApplicationFactory is used to create a test server instance of our application (Program).
    /// The GetAsync method is invoked with a topic id (1) that should always be in the database by default.
    /// The test checks the GetAsync method returns (200 ok) and the returned body text is not empty.
    /// </summary>
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