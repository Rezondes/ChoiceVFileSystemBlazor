using ChoiceVFileSystemBlazor.Digest;
using ChoiceVFileSystemBlazor.Models;
using ChoiceVFileSystemBlazor.Services;
using Microsoft.Extensions.Options;
using Refit;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class ServiceCollectionExtension
{
    public static void ConfigureHttpClient<TClient>(this IServiceCollection services)
        where TClient : class
    {
        services.AddHttpClient<TClient>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ChoiceVApiSettingsModel>>().Value;
                client.BaseAddress = new Uri(settings.BaseAddress);
            })
            .ConfigurePrimaryHttpMessageHandler((sp) =>
            {
                var settings = sp.GetRequiredService<IOptions<ChoiceVApiSettingsModel>>().Value;
                var tokenService = sp.GetRequiredService<TokenService>();
                return new DigestAuthHandler(settings.BasicAuthUsername, settings.BasicAuthPassword, tokenService);
            })
            .AddTypedClient(RestService.For<TClient>);
    }
    
    public static void ConfigureHttpClientForWhitelist<TClient>(this IServiceCollection services)
        where TClient : class
    {
        services.AddHttpClient<TClient>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ChoiceVWhitelistApiSettingsModel>>().Value;
                client.BaseAddress = new Uri(settings.BaseAddress);
            })
            .ConfigurePrimaryHttpMessageHandler((sp) =>
            {
                var settings = sp.GetRequiredService<IOptions<ChoiceVWhitelistApiSettingsModel>>().Value;
                var tokenService = sp.GetRequiredService<TokenService>();
                return new DigestAuthHandler(settings.BasicAuthUsername, settings.BasicAuthPassword, tokenService);
            })
            .AddTypedClient(RestService.For<TClient>);
    }
}