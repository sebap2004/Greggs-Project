namespace SoftwareProject.Client.Backend.Fakes;

/// <summary>
///  A fake message handler which implements HttpMessageHandler interface
///  Used to send fake responses due to limitations of FakeItEasy. HttpClient cannot be faked as it is concrete.
///  FakeHttpMessageHandler is used to incercept a real httpclient request before it is sent to a real API link
/// </summary>
public class FakeHttpMessageHandler(HttpResponseMessage fakeResponse) : HttpMessageHandler
{
    /// <summary>
    /// overrides SenddAsync method to return a response before a push is sent
    /// </summary>
    /// <param name="request">Stores the user input to be sent to the API</param>
    /// <param name="cancellationToken">Not used. Passes the cancellation token of the request</param>
    /// <returns></returns>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(fakeResponse);
    }
}