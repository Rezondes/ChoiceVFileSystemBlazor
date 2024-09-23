using Microsoft.AspNetCore.Authentication;

namespace ChoiceVFileSystemBlazor.Services;

public class TokenService(IHttpContextAccessor httpContextAccessor, IAuthenticationService authenticationService)
{
    public async Task<string?> GetAccessTokenAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var authenticateResult = await httpContext.AuthenticateAsync("Cookies");
        if (!authenticateResult.Succeeded) return null;
        
        var accessToken = authenticateResult.Properties.GetTokenValue("access_token");
        return accessToken;
    }
}