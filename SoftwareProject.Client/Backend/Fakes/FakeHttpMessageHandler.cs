namespace SoftwareProject.Client.Backend.Fakes;
public class FakeHttpMessageHandler(HttpResponseMessage fakeResponse) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(fakeResponse);
    }
}