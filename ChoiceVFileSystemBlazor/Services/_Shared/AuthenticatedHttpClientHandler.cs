using System.Net.Http.Headers;

namespace ChoiceVFileSystemBlazor.Services._Shared;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly string _jwtToken;

    public AuthenticatedHttpClientHandler(string jwtToken)
    {
        _jwtToken = jwtToken;
        InnerHandler = new HttpClientHandler();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        return base.SendAsync(request, cancellationToken);
    }
}
