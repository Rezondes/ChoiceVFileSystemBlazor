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
using ChoiceVFileSystemBlazor.Database.Ranks.Proxies;
using ChoiceVFileSystemBlazor.Database.Ranks.Proxies.Intefaces;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Services;
using ChoiceVRefitClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using Refit;

// TODO remove serverapi and put it to the gameserver
_ = Task.Run(ChoiceVApi.ChoiceVApi.Start);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var jwtEncryptionKey = builder.Configuration.GetValue<string>("Jwt:EncryptionKey");
Assert(string.IsNullOrEmpty(jwtEncryptionKey), $"JWT Encryption Key is missing");
var jwlIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
Assert(string.IsNullOrEmpty(jwlIssuer), $"JWT Issuer is missing");
var jwlAudience = builder.Configuration.GetValue<string>("Jwt:Audience");
Assert(string.IsNullOrEmpty(jwlAudience), $"JWT Audience is missing");

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
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwlIssuer,
        ValidAudience = jwlAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtEncryptionKey!))
    };
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
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                    context.HttpContext.RequestAborted);
                response.EnsureSuccessStatusCode();

                var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

                context.RunClaimActions(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        },
        OnRemoteFailure = context =>
        {
            context.Response.Redirect($"{Error.GetRedirectUrl()}?failureMessage={context.Failure.Message}");
            context.HandleResponse();
            return Task.CompletedTask;
        }
    };
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookieValue = "true";
});
#endregion

#region ChoiceV Api
var choiceVApiBaseAddress = builder.Configuration.GetValue<string>("ChoiceVApi:BaseAddress");
Assert(string.IsNullOrEmpty(choiceVApiBaseAddress), "ChoiceVApi Address is missing");

builder.Services.AddRefitClient<IAccountApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(choiceVApiBaseAddress!));

builder.Services.AddRefitClient<ICharacterApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(choiceVApiBaseAddress!));

builder.Services.AddRefitClient<IInventoryApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(choiceVApiBaseAddress!));
#endregion

#region Database
builder.Services.AddDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(
                builder.Configuration.GetConnectionString("DefaultConnection"))),
    ServiceLifetime.Scoped);

builder.Services.AddScoped<IRankProxy, RankProxy>();
builder.Services.AddScoped<ISupportfileProxy, SupportfileProxy>();
builder.Services.AddScoped<ISupportfileLogsProxy, SupportfileLogsProxy>();
builder.Services.AddScoped<ISupportfileEntryProxy, SupportfileEntryProxy>();
builder.Services.AddScoped<IAccessProxy, AccessProxy>();
builder.Services.AddScoped<IAccessLogsProxy, AccessLogsProxy>();
builder.Services.AddScoped<IDiscordRolesProxy, DiscordRoleProxy>();
builder.Services.AddScoped<IDiscordRoleLogsProxy, DiscordRoleLogsProxy>();
#endregion

builder.Services.AddSingleton<AuthorizationService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(Error.GetRedirectUrl(), createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthorization();
app.UseCookiePolicy();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<BaseHub>(BaseHub.HubPattern);
app.MapHub<SupportfileHub>(SupportfileHub.HubPattern);

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    
    if (response.StatusCode == 404)
    {
        var originalUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        var redirectUrl = $"{NotFound.GetRedirectUrl()}?requestedUrl={Uri.EscapeDataString(originalUrl)}";
        response.Redirect(redirectUrl);
    }
});

app.Run();

void Assert(bool condition, string exceptionMessage)
{
    if (condition) throw new ApplicationException(exceptionMessage);
}