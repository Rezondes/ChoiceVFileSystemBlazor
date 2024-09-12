using ChoiceVFileSystemBlazor.Components.Supportfiles.Enums;
using Microsoft.AspNetCore.SignalR;

namespace ChoiceVFileSystemBlazor.Components.Supportfiles.Hubs;

public class SupportfileHub : Hub
{
    public const string HubPattern = "/supportfilehub";
    
    /// <summary>
    /// SupportfileHubMethodEnum.UpdateEntrys
    /// </summary>
    /// <param name="supportfileId"></param>
    public async Task UpdateEntrys(Ulid supportfileId)
    {
        await Clients.All.SendAsync(SupportfileHubMethodEnum.UpdateEntrys.ToString(), supportfileId);
    }
    
    /// <summary>
    /// SupportfileHubMethodEnum.EntryCreated
    /// </summary>
    public async Task EntryCreated()
    {
        await Clients.All.SendAsync(SupportfileHubMethodEnum.EntryCreated.ToString());
    }
    
    /// <summary>
    /// SupportfileHubMethodEnum.UpdateFile
    /// </summary>
    public async Task UpdateFile(Ulid supportfileId)
    {
        await Clients.All.SendAsync(SupportfileHubMethodEnum.UpdateFile.ToString(), supportfileId);
    }
    
    /// <summary>
    /// SupportfileHubMethodEnum.EntryCreated
    /// </summary>
    public async Task ToggleFileDeleted(Ulid supportfileId)
    {
        await Clients.All.SendAsync(SupportfileHubMethodEnum.ToggleFileDeleted.ToString(), supportfileId);
    }
}