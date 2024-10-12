using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChoiceVFileSystemBlazor.Models;
using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services;

public class DiscordService
{
    private readonly HttpClient _httpClient;
    private readonly DiscordBotSettingsModel _discordSettings;
    private readonly TokenService _tokenService;

    public DiscordService(HttpClient httpClient, IOptions<DiscordBotSettingsModel> discordOptions, TokenService tokenService)
    {
        _httpClient = httpClient;
        _discordSettings = discordOptions.Value;
        _tokenService = tokenService;
    }

    public async Task<List<DiscordMember>> GetGuildMembersAsync()
    {
        var accessToken = await _tokenService.GetAccessTokenAsync();
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://discord.com/api/v10/users/@me/guilds/{_discordSettings.GuildId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {error}");
            return new List<DiscordMember>();
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<DiscordMember>>(jsonResponse) ?? new List<DiscordMember>();
    }
}

public class DiscordMember
{
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("communication_disabled_until")]
    public DateTime? CommunicationDisabledUntil { get; set; }

    [JsonPropertyName("flags")]
    public int Flags { get; set; }

    [JsonPropertyName("joined_at")]
    public DateTime JoinedAt { get; set; }

    [JsonPropertyName("nick")]
    public string? Nick { get; set; }

    [JsonPropertyName("pending")]
    public bool Pending { get; set; }

    [JsonPropertyName("premium_since")]
    public DateTime? PremiumSince { get; set; }

    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = new();

    [JsonPropertyName("unusual_dm_activity_until")]
    public DateTime? UnusualDmActivityUntil { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser User { get; set; }

    [JsonPropertyName("mute")]
    public bool Mute { get; set; }

    [JsonPropertyName("deaf")]
    public bool Deaf { get; set; }

    [JsonPropertyName("bio")]
    public string Bio { get; set; }
}

public class DiscordUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("discriminator")]
    public string Discriminator { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("public_flags")]
    public int PublicFlags { get; set; }

    [JsonPropertyName("flags")]
    public int Flags { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("accent_color")]
    public int? AccentColor { get; set; }

    [JsonPropertyName("global_name")]
    public string? GlobalName { get; set; }

    [JsonPropertyName("avatar_decoration_data")]
    public string? AvatarDecorationData { get; set; }

    [JsonPropertyName("banner_color")]
    public string? BannerColor { get; set; }

    [JsonPropertyName("clan")]
    public string? Clan { get; set; }
}