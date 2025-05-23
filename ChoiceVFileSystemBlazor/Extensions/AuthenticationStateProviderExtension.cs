using System.Security.Claims;
using ChoiceVFileSystemBlazor.Services;
using ChoiceVFileSystemBlazor.Services.Discord;
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

    public static async Task<bool> IsOnDiscordGuildAsync(this AuthenticationStateProvider authenticationStateProvider, DiscordBotService discordBotService)
    {
        var discordId = await authenticationStateProvider.GetDiscordUserIdAsync();
        if (discordId is null) return false;
        
        var user = discordBotService.GetCachedUser(discordId);
        return user is not null;
    }
    
    public static async Task<bool> IsAuthenticatedAndOnDiscordGuildAsync(this AuthenticationStateProvider authenticationStateProvider, DiscordBotService discordBotService)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        if (!UserAccessService.HasClaims(authState.User)) return false;
        
        var discordId = await authenticationStateProvider.GetDiscordUserIdAsync();
        if (discordId is null) return false;
        
        var user = discordBotService.GetCachedUser(discordId);
        return user is not null;
    }

    public static async Task<bool> IsCitizenAsync(this AuthenticationStateProvider authenticationStateProvider, DiscordBotService discordBotService)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        if (!UserAccessService.HasClaims(authState.User)) return false;
        
        var discordId = await authenticationStateProvider.GetDiscordUserIdAsync();
        if (discordId is null) return false;
        
        var user = discordBotService.GetCachedUser(discordId);
        if (user is null) return false;

        var hasCitizenRole = user.HasCitizenRole(discordBotService);
        return hasCitizenRole;
    }
}