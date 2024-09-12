using ChoiceVFileSystemBlazor.Components._Base;
using Microsoft.AspNetCore.Components;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class NavigationManagerExtension
{
    private const string NotAuthorizedUri = "notauthorized";
    
    public static void NavigateToError(this NavigationManager navigation)
    {
        navigation.NavigateTo(Error.GetRedirectUrl(), true);
    }

    public static void NavigateToNotAuthorized(this NavigationManager navigation)
    {
        if (navigation.Uri.EndsWith(NotAuthorizedUri)) return;
        
        navigation.NavigateTo(NotAuthorizedUri, true);
    }
}