using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services.Discord.Authentication;

public class DiscordAuthenticationServiceOptions
{
    public IOptions<DiscordBotSettingsModel> DiscordSettings { get; set; }
}