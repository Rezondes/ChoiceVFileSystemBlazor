using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services.DiscordAuthentication;

public class DiscordAuthenticationServiceOptions
{
    public IOptions<DiscordBotSettingsModel> DiscordSettings { get; set; }
}