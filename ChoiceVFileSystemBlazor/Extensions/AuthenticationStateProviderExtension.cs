using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class AuthenticationStateProviderExtension
{
    public static async Task<ClaimsPrincipal?> GetUser(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }
}