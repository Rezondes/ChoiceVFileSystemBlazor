using ChoiceVFileSystemBlazor.Database.BugTracker.Enums;

namespace ChoiceVFileSystemBlazor.Database.BugTracker.DbModels;

public class BugTrackerTaskItemDbModel
{
    public BugTrackerTaskItemDbModel() { }

    public BugTrackerTaskItemDbModel(
        string title, string description,
        string discordId, string discordName,
        BugTrackerCategoryEnum category, BugTrackerPriorityEnum priority,
        BugTrackerStatusEnum status)
    {
        Title = title;
        Description = description;
        CreatedByDiscordId = discordId;
        CreatedByDiscordName = discordName;
        Category = category;
        Priority = priority;
        Status = status;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public string CreatedByDiscordId { get; set; }
    
    public string CreatedByDiscordName { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }

    public BugTrackerCategoryEnum Category { get; set; }
    
    public BugTrackerPriorityEnum Priority { get; set; }
    
    public BugTrackerStatusEnum Status { get; set; }
    
    public bool Deleted { get; set; } = false;
    
    public List<BugTrackerTaskCommentDbModel> Comments { get; set; } = [];

}