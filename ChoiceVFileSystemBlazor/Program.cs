using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ChoiceVFileSystemBlazor.Components;
using ChoiceVFileSystemBlazor.Components._Base;
using ChoiceVFileSystemBlazor.Components._Layout.Hubs;
using ChoiceVFileSystemBlazor.Components.Supportfiles.Hubs;
using ChoiceVFileSystemBlazor.Database;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.Discord.Proxies;
using ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.News.Proxies;
using ChoiceVFileSystemBlazor.Database.News.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.Ranks.Proxies;
using ChoiceVFileSystemBlazor.Database.Ranks.Proxies.Intefaces;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Extensions;
using ChoiceVFileSystemBlazor.Models;
using ChoiceVFileSystemBlazor.Services;
using ChoiceVFileSystemBlazor.Services.DiscordGuildMembers;
using ChoiceVFileSystemBlazor.Services.Serverinformations;
using ChoiceVRefitClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
    // config.AddEventSourceLogger();
    // config.AddEventLog();
    // config.AddFile("Logs/myapp-{Date}.txt"); 
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddSignalR();

#region MudBlazor Configurations
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 20000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
#endregion

builder.Services.AddDistributedMemoryCache();

#region Discord Authentication

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var jwtEncryptionKey = builder.Configuration.GetValue<string>("Jwt:EncryptionKey");
Assert(string.IsNullOrEmpty(jwtEncryptionKey), $"JWT Encryption Key is missing");
var jwlIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
Assert(string.IsNullOrEmpty(jwlIssuer), $"JWT Issuer is missing");
var jwlAudience = builder.Configuration.GetValue<string>("Jwt:Audience");
Assert(string.IsNullOrEmpty(jwlAudience), $"JWT Audience is missing");

builder.Services.Configure<DiscordBotSettingsModel>(builder.Configuration.GetSection("Discord"));

builder.Services.AddAuthentication(options =>
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

    var discordClientId = builder.Configuration.GetValue<string>("Discord:ClientId");
    Assert(string.IsNullOrEmpty(discordClientId), $"Discord client id is missing");
    var discordClientSecret = builder.Configuration.GetValue<string>("Discord:ClientSecret");
    Assert(string.IsNullOrEmpty(discordClientSecret), $"Discord client secret is missing");
    
    options.ClientId = discordClientId!;
    options.ClientSecret = discordClientSecret!;
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
            Console.WriteLine(context.AccessToken);
            
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

            context.RunClaimActions(user);

            // Get Guild User
            var guildId = builder.Configuration.GetValue<string>("Discord:GuildId");
            Assert(string.IsNullOrEmpty(guildId), "Discord guild id is missing");

            var membersRequest = new HttpRequestMessage(HttpMethod.Get,
                $"https://discord.com/api/v10/users/@me/guilds/{guildId}/member");
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DiscordPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes("Discord");
    });
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookieValue = "yes";
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // Für Cross-Origin-Anfragen
});
#endregion

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<StartupService>();
builder.Services.AddScoped<ReloadService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserAccessService>();
builder.Services.AddScoped<DiscordService>();
builder.Services.AddScoped<PageLoadingService>();

builder.Services.AddSingleton<LockService>();

builder.Services.AddSingleton<ServerInformationCachedService>();
builder.Services.AddHostedService<ServerInformationBackgroundService>();

builder.Services.AddSingleton<DiscordGuildMembersCachedService>();
builder.Services.AddHostedService<DiscordGuildMembersBackgroundService>();

#region ChoiceV Api
var choiceVApiBaseAddress = builder.Configuration.GetValue<string>("ChoiceVApi:BaseAddress")!;
Assert(string.IsNullOrEmpty(choiceVApiBaseAddress), "ChoiceVApi Address is missing");
var choiceVApiUsername = builder.Configuration.GetValue<string>("ChoiceVApi:BasicAuthUsername")!;
Assert(string.IsNullOrEmpty(choiceVApiUsername), "ChoiceVApi BasicAuthUsername is missing");
var choiceVApiPassword = builder.Configuration.GetValue<string>("ChoiceVApi:BasicAuthPassword")!;
Assert(string.IsNullOrEmpty(choiceVApiPassword), "ChoiceVApi BasicAuthPassword is missing");

builder.Services.ConfigureHttpClient<IAccountApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<IBankAccountApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<ICharacterApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<ICompanyApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<IInventoryApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<IServerApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<IVehicleApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
builder.Services.ConfigureHttpClient<IDiscordApi>(choiceVApiBaseAddress, choiceVApiUsername, choiceVApiPassword);
#endregion

#region ChoiceV Whitelist Api
var choiceVWhitelistApiBaseAddress = builder.Configuration.GetValue<string>("ChoiceVWhitelistApi:BaseAddress")!;
Assert(string.IsNullOrEmpty(choiceVWhitelistApiBaseAddress), "ChoiceVWhitelistApi Address is missing");
var choiceVWhitelistApiUsername = builder.Configuration.GetValue<string>("ChoiceVWhitelistApi:BasicAuthUsername")!;
Assert(string.IsNullOrEmpty(choiceVWhitelistApiUsername), "ChoiceVWhitelistApi BasicAuthUsername is missing");
var choiceVWhitelistApiPassword = builder.Configuration.GetValue<string>("ChoiceVWhitelistApi:BasicAuthPassword")!;
Assert(string.IsNullOrEmpty(choiceVWhitelistApiPassword), "ChoiceVWhitelistApi BasicAuthPassword is missing");

builder.Services.ConfigureHttpClient<IWhitelistQuestionApi>(choiceVWhitelistApiBaseAddress, choiceVWhitelistApiUsername, choiceVWhitelistApiPassword);
builder.Services.ConfigureHttpClient<IWhitelistProcedureApi>(choiceVWhitelistApiBaseAddress, choiceVWhitelistApiUsername, choiceVWhitelistApiPassword);
#endregion

#region Database
builder.Services.AddDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(
                builder.Configuration.GetConnectionString("DefaultConnection"))),
    ServiceLifetime.Scoped);

builder.Services.AddScoped<IAccessProxy, AccessProxy>();
builder.Services.AddScoped<IRankProxy, RankProxy>();
builder.Services.AddScoped<IFileProxy, FileProxy>();
builder.Services.AddScoped<IFileCategoryProxy, FileCategoryProxy>();
builder.Services.AddScoped<IFileLogsProxy, FileLogsProxy>();
builder.Services.AddScoped<IFileEntryProxy, FileEntryProxy>();
builder.Services.AddScoped<IAccessLogsProxy, AccessLogsProxy>();
builder.Services.AddScoped<IDiscordRolesProxy, DiscordRoleProxy>();
builder.Services.AddScoped<IDiscordRoleLogsProxy, DiscordRoleLogsProxy>();
builder.Services.AddScoped<INewsProxy, NewsProxy>();
#endregion

if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    });
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(Error.GetRedirectUrl(), createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<BaseHub>(BaseHub.HubPattern);
app.MapHub<FileHub>(FileHub.HubPattern);

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode != 404) return Task.CompletedTask;
    
    var originalUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
    var redirectUrl = $"{NotFound.GetRedirectUrl()}?requestedUrl={Uri.EscapeDataString(originalUrl)}";
    response.Redirect(redirectUrl);

    return Task.CompletedTask;
});

app.Run();

void Assert(bool condition, string exceptionMessage)
{
    if (condition) throw new ApplicationException(exceptionMessage);
}