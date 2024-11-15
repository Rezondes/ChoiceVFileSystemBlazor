using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services.Discord;

public class DiscordBotService : IHostedService
{
    private readonly ILogger<DiscordBotService> _logger;
    private readonly DiscordBotSettingsModel _settings;
    private readonly DiscordSocketClient _client;
    private readonly Dictionary<ulong, SocketGuildUser> _guildUserCache = new();

    public DiscordBotService(ILogger<DiscordBotService> logger, IOptions<DiscordBotSettingsModel> discordSettings)
    {
        _logger = logger;
        _settings = discordSettings.Value;
        
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMembers 
        };
        
        _client = new DiscordSocketClient(config);
    }
    
    public IReadOnlyDictionary<ulong, SocketGuildUser> GetGuildUserCache()
    {
        return _guildUserCache;
    }

    public SocketGuildUser? GetCachedUser(string userId)
    {
        if (!ulong.TryParse(userId, out var userIdUlong)) return null;
        
        _guildUserCache.TryGetValue(userIdUlong, out var user);
        return user;
    }
    
    public SocketGuildUser? GetCachedUser(ulong userId)
    {
        _guildUserCache.TryGetValue(userId, out var user);
        return user;
    }

    public ulong GetCitizenRoleId()
    {
        return _settings.CitizenRoleId;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Log += LogAsync;
        _client.Ready += OnReadyAsync;
        _client.UserJoined += OnUserJoinedAsync;
        _client.UserLeft += OnUserLeftAsync;

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
    
    public async Task<bool> ValidateDiscordId(string discordId)
    {
        if (!ulong.TryParse(discordId, out var discordIdUlong)) return false;
        
        var user = await _client.GetUserAsync(discordIdUlong);
        
        return user != null;
    }

    private Task LogAsync(LogMessage message)
    {
        _logger.LogInformation(message.ToString());
        return Task.CompletedTask;
    }

    private async Task OnReadyAsync()
    {
        _logger.LogInformation("Bot ist bereit und verbunden.");
        
        var guild = _client.GetGuild(ulong.Parse(_settings.GuildId));
        if (guild != null)
        {
            await guild.DownloadUsersAsync();
            
            foreach (var user in guild.Users)
            {
                _guildUserCache[user.Id] = user;
            }
            
            _logger.LogInformation("Alle Mitglieder der Guild wurden gecached.");
        }
        else
        {
            _logger.LogWarning("Guild mit der angegebenen GuildId nicht gefunden.");
        }
    }
    
    private Task OnUserJoinedAsync(SocketGuildUser user)
    {
        if (user.Guild.Id == ulong.Parse(_settings.GuildId))
        {
            _guildUserCache[user.Id] = user;
            _logger.LogInformation($"Benutzer {user.Username} (ID: {user.Id}) ist der Guild beigetreten und wurde gecached.");
        }
        return Task.CompletedTask;
    }

    private Task OnUserLeftAsync(SocketGuild guild, SocketUser user)
    {
        if (guild.Id == ulong.Parse(_settings.GuildId) && _guildUserCache.ContainsKey(user.Id))
        {
            _guildUserCache.Remove(user.Id);
            _logger.LogInformation($"Benutzer {user.Username} (ID: {user.Id}) hat die Guild verlassen und wurde aus dem Cache entfernt.");
        }
        return Task.CompletedTask;
    }

    public async Task<bool> SendNewMessageInfoToUserAsync(string discordId)
    {
        return await SendMessageToUserAsync(discordId, "Du hast eine neue Nachricht im User Control Panel! Schau jetzt nach auf https://ucp.choicev.net.");
    }
    
    public async Task<bool> SendMessageToUserAsync(string discordId, string message)
    {
        if (!ulong.TryParse(discordId, out var userId))
        {
            _logger.LogWarning($"Die angegebene DiscordId '{discordId}' ist ungültig.");
            return false;
        }

        if (!_guildUserCache.TryGetValue(userId, out var user))
        {
            _logger.LogWarning($"Benutzer mit der DiscordId {discordId} ist nicht in der Guild oder nicht gecached.");
            return false;
        }

        try
        {
            var dmChannel = await user.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync(message);

            _logger.LogInformation($"Nachricht an Benutzer {user.Username} (ID: {userId}) gesendet.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fehler beim Senden der Nachricht an Benutzer {discordId}.");
            return false;
        }
    }
}