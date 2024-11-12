using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ChoiceVFileSystemBlazor.Components._Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services.DiscordAuthentication;

public class DiscordAuthenticationService
{
    private readonly DiscordBotSettingsModel _discordSettings;

    public DiscordAuthenticationService(IOptions<DiscordBotSettingsModel> discordSettings)
    {
        _discordSettings = discordSettings.Value;
    }

    public void Configure(IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(2);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "Discord";
        })
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(6);
        })
        .AddOAuth("Discord", options =>
        {
            options.AuthorizationEndpoint = "https://discord.com/oauth2/authorize";
            options.Scope.Add("identify");
            options.Scope.Add("guilds");
            options.Scope.Add("guilds.members.read");

            options.CallbackPath = new PathString("/api/discord/auth/oauthCallback");

            // Discord-Client-Informationen aus den Einstellungen
            options.ClientId = _discordSettings.ClientId;
            options.ClientSecret = _discordSettings.ClientSecret;
            options.SaveTokens = true;

            options.TokenEndpoint = "https://discord.com/api/oauth2/token";
            options.UserInformationEndpoint = "https://discord.com/api/users/@me";

            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            options.ClaimActions.MapJsonKey(ClaimTypes.Role, "roles");

            options.AccessDeniedPath = Error.GetRedirectUrl();

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                        context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
                    context.RunClaimActions(user);
                    
                    var membersRequest = new HttpRequestMessage(HttpMethod.Get,
                        $"https://discord.com/api/v10/users/@me/guilds/{_discordSettings.GuildId}/member");
                    membersRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    membersRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    var membersResponse = await context.Backchannel.SendAsync(membersRequest,
                        HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                    membersResponse.EnsureSuccessStatusCode();

                    var memberJson = await membersResponse.Content.ReadAsStringAsync();
                    var member = JsonDocument.Parse(memberJson).RootElement;
                    context.RunClaimActions(member);
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("DiscordPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes("Discord");
            });
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.ConsentCookieValue = "yes";
        });
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
        });
    }
}