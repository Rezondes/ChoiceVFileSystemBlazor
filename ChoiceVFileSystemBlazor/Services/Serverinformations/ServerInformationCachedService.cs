using ChoiceVRefitClient;
using ChoiceVSharedApiModels.Server;

namespace ChoiceVFileSystemBlazor.Services.Serverinformations;

public class ServerInformationCachedService(IServerApi serverApi, ILogger<ServerInformationCachedService> logger, LockService lockService)
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private CurrentServerInfosApiModel? _cachedData;
    private DateTime? _cachedLastUpdate;
    private DateTime? _lastTry;
    private bool? _lastTrySuccess;

    public async Task FetchDataFromApi()
    {
        logger.LogInformation("Fetching server infos....");
        _lastTry = DateTime.UtcNow;
        
        try
        {
            var response = await serverApi.GetCurrentServerInfosAsync();
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

    public async Task<(bool?, DateTime?, DateTime?, CurrentServerInfosApiModel?)> GetCachedData()
    {
        return await lockService.LockAsync(() => Task.FromResult((_lastTrySuccess, _lastTry, _cachedLastUpdate, _cachedData)));
    }
}