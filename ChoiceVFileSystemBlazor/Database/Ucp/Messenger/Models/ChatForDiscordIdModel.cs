namespace ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Models;

public class ChatForDiscordIdModel
{
    public string DiscordId { get; set; }
    public int MessageCount { get; set; }
    public DateTime LastMessageSent { get; set; }
}