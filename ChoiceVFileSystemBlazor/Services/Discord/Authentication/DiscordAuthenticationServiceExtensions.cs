namespace ChoiceVFileSystemBlazor.Services.Discord.Authentication;

public static class DiscordAuthenticationServiceExtensions
{
    public static IServiceCollection AddDiscordAuthentication(this IServiceCollection services,
        Action<DiscordAuthenticationServiceOptions> configureOptions)
    {
        var options = new DiscordAuthenticationServiceOptions();
        configureOptions(options);

        var discordAuthService = new DiscordAuthenticationService(options.DiscordSettings);
        discordAuthService.Configure(services);

        return services;
    }
}