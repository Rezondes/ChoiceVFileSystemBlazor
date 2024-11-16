namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Model;

public class BugTaskModel
{
    public BugTaskModel(Ulid id, string discordId, string discordName, int bugTaskId, string bugTaskName)
    {
        Id = id;
        DiscordId = discordId;
        DiscordName = discordName;
        BugTaskId = bugTaskId;
        BugTaskName = bugTaskName;
    }

    public Ulid Id { get; set; }
    
    public string DiscordId { get; set; }
    
    public string DiscordName { get; set; }
    
    public int BugTaskId { get; set; }
    
    public string BugTaskName { get; set; }
}