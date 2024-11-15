namespace ChoiceVFileSystemBlazor.Database.Ucp.Messenger.DbModels;

public class MessageToDiscordIdDbModel
{
    public MessageToDiscordIdDbModel()
    {
    }

    public MessageToDiscordIdDbModel(string toDiscordId, string message, bool isFromUser, string creatorName, Ulid? scpUserId = null)
    {
        ToDiscordId = toDiscordId;
        IsFromUser = isFromUser;
        Message = message;
        CreatorName = creatorName;
        ScpUserId = scpUserId;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public bool IsReadByUser { get; set; }

    public string ToDiscordId { get; set; }
    
    public bool IsFromUser { get; set; }

    public string Message { get; set; }

    public string CreatorName { get; set; }
    
    public Ulid? ScpUserId { get; set; }
}
