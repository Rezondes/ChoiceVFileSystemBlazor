namespace ChoiceVFileSystemBlazor.Services.Discord;

public class DiscordBotSettingsModel
{
    public string ClientId { get; set; }     
    public string ClientSecret { get; set; } 
    public string BotToken { get; set; }
    public string GuildId { get; set; }
    public ulong CitizenRoleId { get; set; }
    
    public DiscordBotSettingsModel(){}
}