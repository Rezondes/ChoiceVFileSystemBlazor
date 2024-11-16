using ChoiceVFileSystemBlazor.Services.Vikunja;
using ChoiceVFileSystemBlazor.Services.Vikunja.Models;

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

public enum BugTaskStatus
{
    Pending,
    Rejected,
    Accepted,
    Done
}

public static class DiscordIdToBugTaskIdDbModelExtensions
{
    public static async Task<BugTaskStatus> GetStatus(this DiscordIdToBugTaskIdDbModel dbTask, 
        VikunjaClientService vikunjaClientService, List<VikunjaTask> allScpBugs, List<VikunjaTask> allChoiceVBugs)
    {
        if (allScpBugs.Select(x => x.Id).Contains(dbTask.BugTaskId))
            return BugTaskStatus.Pending;

        if (!allChoiceVBugs.Select(x => x.Id).Contains(dbTask.BugTaskId)) 
            return BugTaskStatus.Rejected;
        
        var result = await vikunjaClientService.Client.HandleApiRequestAsync(
            async _ => await vikunjaClientService.Client.GetTaskByIdAsync(dbTask.BugTaskId));

        return result.Data!.Done ? BugTaskStatus.Done : BugTaskStatus.Accepted;
    }
    
    public static async Task<BugTaskStatus> GetStatus(this DiscordIdToBugTaskIdDbModel dbTask, VikunjaClientService vikunjaClientService)
    {
        var allScpBugs = await vikunjaClientService.Client.GetAllTasksInProjectAsync(vikunjaClientService.ScpBugsProjectId);
        if (allScpBugs.Select(x => x.Id).Contains(dbTask.BugTaskId))
            return BugTaskStatus.Pending;

        var allChoiceVBugs = await vikunjaClientService.Client.GetAllTasksInProjectAsync(vikunjaClientService.ChoiceVBugsProjectId);
        if (!allChoiceVBugs.Select(x => x.Id).Contains(dbTask.BugTaskId)) 
            return BugTaskStatus.Rejected;
        
        var result = await vikunjaClientService.Client.HandleApiRequestAsync(
            async _ => await vikunjaClientService.Client.GetTaskByIdAsync(dbTask.BugTaskId));

        return result.Data!.Done ? BugTaskStatus.Done : BugTaskStatus.Accepted;
    }
}