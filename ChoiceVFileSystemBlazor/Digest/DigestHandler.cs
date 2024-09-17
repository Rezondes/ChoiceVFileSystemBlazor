using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace ChoiceVFileSystemBlazor.Digest;

public class DigestHandler : DelegatingHandler
{
    private readonly string _username;
    private readonly string _password;

    public DigestHandler(HttpMessageHandler innerHandler, string username, string password)
        : base(innerHandler)
    {
        _username = username;
        _password = password;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Initiale Anfrage senden
        var initialResponse = await base.SendAsync(request, cancellationToken);

        if (initialResponse.StatusCode != HttpStatusCode.Unauthorized) return initialResponse;
        
        // Digest-Authentifizierungs-Challenge erhalten
        var authenticateHeader = initialResponse.Headers.WwwAuthenticate.FirstOrDefault();
        if (authenticateHeader == null || !authenticateHeader.Scheme.Equals("Digest", StringComparison.OrdinalIgnoreCase))
        {
            return initialResponse; // Challenge war keine Digest-Authentifizierung
        }

        if (authenticateHeader.Parameter == null) return initialResponse;
        
        var digestHeader = ParseDigestHeader(authenticateHeader.Parameter);

        if (digestHeader == null) return initialResponse;
        
        // Digest Authorization Header erstellen
        var digestAuthHeader = CreateDigestAuthHeader(digestHeader, request.Method.Method, request.RequestUri!);
        request.Headers.Authorization = new AuthenticationHeaderValue("Digest", digestAuthHeader);

        // Anfrage erneut senden mit Digest Authentication Header
        return await base.SendAsync(request, cancellationToken);
    }

    private DigestHeaderParameters? ParseDigestHeader(string digestHeader)
    {
        return new DigestHeaderParameters(digestHeader);
    }

    private string CreateDigestAuthHeader(DigestHeaderParameters digestParams, string httpMethod, Uri uri)
    {
        var ha1 = CalculateMd5Hash($"{_username}:{digestParams.Realm}:{_password}");
        var ha2 = CalculateMd5Hash($"{httpMethod}:{uri.PathAndQuery}");
        var response = CalculateMd5Hash($"{ha1}:{digestParams.Nonce}:{ha2}");

        var authHeaderValue = new StringBuilder();
        authHeaderValue.Append($"username=\"{_username}\", ");
        authHeaderValue.Append($"realm=\"{digestParams.Realm}\", ");
        authHeaderValue.Append($"nonce=\"{digestParams.Nonce}\", ");
        authHeaderValue.Append($"uri=\"{uri.PathAndQuery}\", ");
        authHeaderValue.Append($"response=\"{response}\", ");
        authHeaderValue.Append($"opaque=\"{digestParams.Opaque}\"");

        return authHeaderValue.ToString();
    }

    private static string CalculateMd5Hash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
public class DigestHeaderParameters
{
    public string Realm { get; }
    public string Nonce { get; }
    public string Opaque { get; }

    public DigestHeaderParameters(string digestHeader)
    {
        Realm = GetParameterValue(digestHeader, "realm");
        Nonce = GetParameterValue(digestHeader, "nonce");
        Opaque = GetParameterValue(digestHeader, "opaque");
    }

    private string GetParameterValue(string digestHeader, string parameter)
    {
        var startIndex = digestHeader.IndexOf($"{parameter}=\"", StringComparison.OrdinalIgnoreCase);
        if (startIndex < 0)
        {
            return string.Empty;
        }

        startIndex += parameter.Length + 2;
        var endIndex = digestHeader.IndexOf("\"", startIndex);
        if (endIndex < 0)
        {
            return string.Empty;
        }

        return digestHeader.Substring(startIndex, endIndex - startIndex);
    }
}
