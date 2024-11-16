using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Model;

public class BugTaskModel
{
    public BugTaskModel(Ulid id, string discordId, string discordName, int bugTaskId, string bugTaskName, BugTaskStatus status)
    {
        Id = id;
        DiscordId = discordId;
        DiscordName = discordName;
        BugTaskId = bugTaskId;
        BugTaskName = bugTaskName;
        Status = status;
    }

    public Ulid Id { get; set; }
    
    public string DiscordId { get; set; }
    
    public string DiscordName { get; set; }
    
    public int BugTaskId { get; set; }
    
    public string BugTaskName { get; set; }
    
    public BugTaskStatus Status { get; set; }
}