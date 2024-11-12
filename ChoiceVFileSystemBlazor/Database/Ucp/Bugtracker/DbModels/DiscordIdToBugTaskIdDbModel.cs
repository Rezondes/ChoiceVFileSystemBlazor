namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;

public class DiscordIdToBugTaskIdDbModel
{
    public DiscordIdToBugTaskIdDbModel(){}

    public DiscordIdToBugTaskIdDbModel(string discordId, int bugTaskId, string bugTaskName)
    {
        DiscordId = discordId;
        BugTaskId = bugTaskId;
        BugTaskName = bugTaskName;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public string DiscordId { get; set; }
    
    public int BugTaskId { get; set; }
    
    public string BugTaskName { get; set; }
}