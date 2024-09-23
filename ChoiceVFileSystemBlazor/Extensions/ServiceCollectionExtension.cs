using ChoiceVFileSystemBlazor.Digest;
using ChoiceVFileSystemBlazor.Services;
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
            .ConfigurePrimaryHttpMessageHandler((sp) =>
            {
                var tokenService = sp.GetRequiredService<TokenService>();
                return new DigestAuthHandler(username, password, tokenService);
            })
            .AddTypedClient(RestService.For<TClient>);
    }
}