using ChoiceVRefitClient;
using ChoiceVSharedApiModels.Discord;

namespace ChoiceVFileSystemBlazor.Services.DiscordGuildMembers;

public class DiscordGuildMembersCachedService(IDiscordApi discordApi, ILogger<DiscordGuildMembersCachedService> logger, LockService lockService)
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private List<DiscordUserApiModel>? _cachedData;
    private DateTime? _cachedLastUpdate;
    private DateTime? _lastTry;
    private bool? _lastTrySuccess;

    public async Task FetchDataFromApi()
    {
        logger.LogInformation("Fetching server infos....");
        _lastTry = DateTime.UtcNow;
        
        try
        {
            var response = await discordApi.GetAllDiscordGuildMembersAsync();
            if (response.IsSuccessStatusCode)
            {
                await lockService.LockAsync(() =>
                {
                    _cachedData = response.Content;
                    _cachedLastUpdate = DateTime.UtcNow;
                    _lastTrySuccess = true;
                    logger.LogInformation("Server infos fetching successful.");
                    
                    return Task.FromResult(Task.CompletedTask);
                });
            }
            else
            {
                logger.LogError(response.Error, "Server infos fetching failed.");
            }
        }
        catch (HttpRequestException ex)
        {
            _lastTrySuccess = false;
            logger.LogError(ex, "Failed to fetch data from server");
        }
        catch (Exception ex)
        {
            _lastTrySuccess = false;
            logger.LogError(ex, "Failed to fetch data from server");
        }
    }

    public async Task<(bool?, DateTime?, DateTime?, List<DiscordUserApiModel>?)> GetCachedData()
    {
        return await lockService.LockAsync(() => Task.FromResult((_lastTrySuccess, _lastTry, _cachedLastUpdate, _cachedData)));
    }
}