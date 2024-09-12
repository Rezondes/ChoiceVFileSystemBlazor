using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChoiceVFileSystemBlazor.Helper;

public static class HubHelper
{
    public static HubConnection GetHubConnection(NavigationManager navigation, string hubPattern)
    {
        return new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri(hubPattern))
            .Build();
    }
}