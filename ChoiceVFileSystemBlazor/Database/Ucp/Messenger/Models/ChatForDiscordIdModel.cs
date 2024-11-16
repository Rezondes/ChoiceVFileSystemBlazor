namespace ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Models;

public class ChatForDiscordIdModel
{
    public ChatForDiscordIdModel(string discordId, string discordName, int messageCount, DateTime lastMessageSent)
    {
        DiscordId = discordId;
        DiscordName = discordName;
        MessageCount = messageCount;
        LastMessageSent = lastMessageSent;
    }

    public string DiscordId { get; set; }
    public string DiscordName { get; set; }
    public int MessageCount { get; set; }
    public DateTime LastMessageSent { get; set; }
}