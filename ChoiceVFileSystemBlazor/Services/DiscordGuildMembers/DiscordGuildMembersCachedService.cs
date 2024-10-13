using ChoiceVRefitClient;
using ChoiceVSharedApiModels.Discord;

namespace ChoiceVFileSystemBlazor.Services.DiscordGuildMembers;

public class DiscordGuildMembersCachedService(IDiscordApi discordApi, ILogger<DiscordGuildMembersCachedService> logger)
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly object _dataLock = new();
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
                lock (_dataLock)
                {
                    _cachedData = response.Content;
                    _cachedLastUpdate = DateTime.UtcNow;
                    _lastTrySuccess = true;
                    logger.LogInformation("Server infos fetching successful.");
                }
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
        await _semaphore.WaitAsync();
        try
        {
            return (_lastTrySuccess, _lastTry, _cachedLastUpdate, _cachedData);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}