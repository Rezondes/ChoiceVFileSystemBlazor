using ChoiceVFileSystemBlazor.Components._Layout.Enums;
using Microsoft.AspNetCore.SignalR;

namespace ChoiceVFileSystemBlazor.Components._Layout.Hubs;

public class BaseHub : Hub
{
    public const string HubPattern = "/base";
    
    /// <summary>
    /// BaseHubMethodEnum.UpdateAccess
    /// </summary>
    public async Task UpdateAccess(Ulid accessId)
    {
        await Clients.All.SendAsync(BaseHubMethodEnum.UpdateAccess.ToString(), accessId);
    }
}