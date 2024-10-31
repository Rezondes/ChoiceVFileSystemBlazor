using ChoiceVFileSystemBlazor.Database.BugTracker.DbModels;
using Microsoft.AspNetCore.SignalR;

namespace ChoiceVFileSystemBlazor.Components._BugTracker.Hubs;

public class BugTrackerHub : Hub
{
    public const string HubPattern = "/bugtrackerHub";
    
    public async Task SendTaskUpdate(BugTrackerTaskItemDbModel task)
    {
        await Clients.All.SendAsync("ReceiveTaskUpdate", task);
    }
}