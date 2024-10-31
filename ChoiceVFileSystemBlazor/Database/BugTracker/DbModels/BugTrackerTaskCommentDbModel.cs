namespace ChoiceVFileSystemBlazor.Database.BugTracker.DbModels;

public class BugTrackerTaskCommentDbModel
{
    public BugTrackerTaskCommentDbModel() { }
    
    public BugTrackerTaskCommentDbModel(Ulid taskId, string message, string discordId, string discordName)
    {
        TaskId = taskId;
        Message = message;
        DiscordId = discordId;
        DiscordName = discordName;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public Ulid TaskId { get; set; }
    
    public string Message { get; set; }
    
    public string DiscordId { get; set; }
    
    public string DiscordName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool Deleted { get; set; } = false;
    
    public BugTrackerTaskItemDbModel Task { get; set; }
}