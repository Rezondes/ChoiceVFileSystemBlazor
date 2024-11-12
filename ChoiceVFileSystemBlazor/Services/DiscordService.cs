using System.Net.Http.Headers;
using ChoiceVFileSystemBlazor.Services.DiscordAuthentication;
using Microsoft.Extensions.Options;

namespace ChoiceVFileSystemBlazor.Services;

public class DiscordService
{
    private readonly HttpClient _httpClient;
    private readonly DiscordBotSettingsModel _discordSettings;

    public DiscordService(HttpClient httpClient, IOptions<DiscordBotSettingsModel> discordOptions)
    {
        _httpClient = httpClient;
        _discordSettings = discordOptions.Value;
    }

    public async Task<bool> ValidateDiscordId(string discordId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://discord.com/api/v10/users/{discordId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", _discordSettings.BotToken);

        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}