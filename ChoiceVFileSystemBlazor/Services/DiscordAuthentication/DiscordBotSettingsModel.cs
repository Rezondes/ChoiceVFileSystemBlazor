namespace ChoiceVFileSystemBlazor.Services.DiscordAuthentication;

public class DiscordBotSettingsModel
{
    public string ClientId { get; set; }     
    public string ClientSecret { get; set; } 
    public string GuildId { get; set; }
    public string BotToken { get; set; }
    
    public DiscordBotSettingsModel(){}
}