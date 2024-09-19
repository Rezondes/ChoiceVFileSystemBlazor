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
    
    private static readonly Dictionary<Ulid, Ulid> LockedFiles = new Dictionary<Ulid, Ulid>();

    public async Task LockFile(Ulid supportfileId, Ulid userId)
    {
        if (!LockedFiles.TryAdd(supportfileId, userId)) return;
        
        await Clients.Others.SendAsync("FileLocked", supportfileId, userId);
    }

    public async Task UnlockFile(Ulid supportfileId, Ulid userId)
    {
        if (!LockedFiles.TryGetValue(supportfileId, out var value) || value != userId) return;
       
        LockedFiles.Remove(supportfileId);
        await Clients.Others.SendAsync("FileUnlocked", supportfileId, userId);
    }

    public Task<bool> IsFileLocked(Ulid supportfileId)
    {
        return Task.FromResult(LockedFiles.ContainsKey(supportfileId));
    }

    public Task<Ulid> GetLockedByUser(Ulid supportfileId)
    {
        return Task.FromResult(LockedFiles.GetValueOrDefault(supportfileId));
    }
}