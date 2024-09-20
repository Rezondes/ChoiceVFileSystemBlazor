using ChoiceVFileSystemBlazor.Digest;
using Refit;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class ServiceCollectionExtension
{
    public static void ConfigureHttpClient<TClient>(this IServiceCollection services, string baseAddress, string username, string password)
        where TClient : class
    {
        services.AddHttpClient<TClient>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new DigestAuthHandler(new HttpClientHandler(), username, password))
            .AddTypedClient(RestService.For<TClient>);
    }
}