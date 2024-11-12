using ChoiceVFileSystemBlazor.Services.DiscordAuthentication;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services;

public class DiscordBotService : IHostedService
{
    private readonly ILogger<DiscordBotService> _logger;
    private readonly DiscordBotSettingsModel _settings;
    private readonly DiscordSocketClient _client;

    public DiscordBotService(ILogger<DiscordBotService> logger, IOptions<DiscordBotSettingsModel> discordSettings)
    {
        _logger = logger;
        _settings = discordSettings.Value;
        _client = new DiscordSocketClient();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Log += LogAsync;
        _client.Ready += OnReadyAsync;

        await _client.LoginAsync(TokenType.Bot, _settings.BotToken);
        await _client.StartAsync();

        _logger.LogInformation("DiscordBotService gestartet.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DiscordBotService wird gestoppt.");
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    private Task LogAsync(LogMessage message)
    {
        _logger.LogInformation(message.ToString());
        return Task.CompletedTask;
    }

    private Task OnReadyAsync()
    {
        _logger.LogInformation("Bot ist bereit und verbunden.");
        return Task.CompletedTask;
    }
}