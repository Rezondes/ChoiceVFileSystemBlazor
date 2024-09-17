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

        if (initialResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Digest-Authentifizierungs-Challenge erhalten
            var authenticateHeader = initialResponse.Headers.WwwAuthenticate.FirstOrDefault();
            if (authenticateHeader == null || !authenticateHeader.Scheme.Equals("Digest", StringComparison.OrdinalIgnoreCase))
            {
                return initialResponse; // Challenge war keine Digest-Authentifizierung
            }

            var digestHeader = ParseDigestHeader(authenticateHeader.Parameter);

            if (digestHeader != null)
            {
                // Digest Authorization Header erstellen
                var digestAuthHeader = CreateDigestAuthHeader(digestHeader, request.Method.Method, request.RequestUri!);
                request.Headers.Authorization = new AuthenticationHeaderValue("Digest", digestAuthHeader);

                // Anfrage erneut senden mit Digest Authentication Header
                return await base.SendAsync(request, cancellationToken);
            }
        }

        return initialResponse;
    }

    private DigestHeaderParameters? ParseDigestHeader(string digestHeader)
    {
        return new DigestHeaderParameters(digestHeader);
    }

    private string CreateDigestAuthHeader(DigestHeaderParameters digestParams, string httpMethod, Uri uri)
    {
        var ha1 = CalculateMD5Hash($"{_username}:{digestParams.Realm}:{_password}");
        var ha2 = CalculateMD5Hash($"{httpMethod}:{uri.PathAndQuery}");
        var response = CalculateMD5Hash($"{ha1}:{digestParams.Nonce}:{ha2}");

        var authHeaderValue = new StringBuilder();
        authHeaderValue.AppendFormat("username=\"{0}\", ", _username);
        authHeaderValue.AppendFormat("realm=\"{0}\", ", digestParams.Realm);
        authHeaderValue.AppendFormat("nonce=\"{0}\", ", digestParams.Nonce);
        authHeaderValue.AppendFormat("uri=\"{0}\", ", uri.PathAndQuery);
        authHeaderValue.AppendFormat("response=\"{0}\", ", response);
        authHeaderValue.AppendFormat("opaque=\"{0}\"", digestParams.Opaque);

        return authHeaderValue.ToString();
    }

    private static string CalculateMD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
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
