using System.Security.Cryptography.X509Certificates;
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
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Extensions;
using ChoiceVFileSystemBlazor.Models;
using ChoiceVFileSystemBlazor.Services;
using ChoiceVFileSystemBlazor.Services.Discord;
using ChoiceVFileSystemBlazor.Services.Discord.Authentication;
using ChoiceVFileSystemBlazor.Services.Serverinformations;
using ChoiceVFileSystemBlazor.Services.Vikunja;
using ChoiceVRefitClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddSignalR();

#region MudBlazor Configurations
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 60000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
#endregion

builder.Services.AddDistributedMemoryCache();

#region Discord
builder.Services.Configure<DiscordBotSettingsModel>(builder.Configuration.GetSection("Discord"));
builder.Services.AddDiscordAuthentication(options =>
{
    options.DiscordSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<DiscordBotSettingsModel>>();
});
builder.Services.AddSingleton<DiscordBotService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DiscordBotService>());
#endregion

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<StartupService>();
builder.Services.AddScoped<ReloadService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserAccessService>();
builder.Services.AddScoped<PageLoadingService>();

builder.Services.AddSingleton<LockService>();

builder.Services.AddSingleton<ServerInformationCachedService>();
builder.Services.AddHostedService<ServerInformationBackgroundService>();

#region ChoiceV Api
builder.Services.Configure<ChoiceVApiSettingsModel>(builder.Configuration.GetSection("ChoiceVApi"));

builder.Services.ConfigureHttpClient<IAccountApi>();
builder.Services.ConfigureHttpClient<IBankAccountApi>();
builder.Services.ConfigureHttpClient<ICharacterApi>();
builder.Services.ConfigureHttpClient<ICompanyApi>();
builder.Services.ConfigureHttpClient<IInventoryApi>();
builder.Services.ConfigureHttpClient<IServerApi>();
builder.Services.ConfigureHttpClient<IVehicleApi>();
builder.Services.ConfigureHttpClient<ISupportKeyInfoApi>();
#endregion

#region ChoiceV Whitelist Api
builder.Services.Configure<ChoiceVWhitelistApiSettingsModel>(builder.Configuration.GetSection("ChoiceVWhitelistApi"));

builder.Services.ConfigureHttpClientForWhitelist<IWhitelistQuestionApi>();
builder.Services.ConfigureHttpClientForWhitelist<IWhitelistProcedureApi>();
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
builder.Services.AddScoped<IBugtrackerProxy, BugtrackerProxy>();
#endregion

builder.Services.Configure<VikunjaSettings>(builder.Configuration.GetSection("VikunjaApi"));
builder.Services.AddScoped<VikunjaClientService>();

if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080);
        options.ListenAnyIP(8081); 
    });
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(Error.GetRedirectUrl(), createScopeForErrors: true);
    app.UseHsts();
}

app.UseCookiePolicy();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAllOrigins");


app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<BaseHub>(BaseHub.HubPattern);
    endpoints.MapHub<FileHub>(FileHub.HubPattern);
});

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