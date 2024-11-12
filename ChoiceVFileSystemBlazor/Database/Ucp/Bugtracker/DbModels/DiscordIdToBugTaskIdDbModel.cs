namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;

public class DiscordIdToBugTaskIdDbModel
{
    public DiscordIdToBugTaskIdDbModel(){}

    public DiscordIdToBugTaskIdDbModel(string discordId, int bugTaskId)
    {
        DiscordId = discordId;
        BugTaskId = bugTaskId;
    }

    public Ulid Id { get; set; }
    
    public string DiscordId { get; set; }
    
    public int BugTaskId { get; set; }
    
    public string BugTaskName { get; set; }
}