using System.Security.Claims;
using ChoiceVFileSystemBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class AuthenticationStateProviderExtension
{
    public static async Task<string?> GetDiscordUserIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        if (!UserAccessService.HasClaims(authState.User)) return null;
        
        return authState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    
    public static async Task<string?> GetDiscordUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        if (!UserAccessService.HasClaims(authState.User)) return null;

        return authState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
    }
        
    public static async Task<bool> IsAuthenticatedAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        return UserAccessService.HasClaims(authState.User);
    }
}