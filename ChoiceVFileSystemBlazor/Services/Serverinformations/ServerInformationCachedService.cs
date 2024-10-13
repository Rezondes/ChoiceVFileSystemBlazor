using ChoiceVRefitClient;
using ChoiceVSharedApiModels.Server;

namespace ChoiceVFileSystemBlazor.Services.Serverinformations;

public class ServerInformationCachedService(IServerApi serverApi, ILogger<ServerInformationCachedService> logger)
{
    private readonly object _dataLock = new();
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

    public (bool?, DateTime?, DateTime?, CurrentServerInfosApiModel?) GetCachedData()
    {
        lock (_dataLock)
        {
            return (_lastTrySuccess, _lastTry, _cachedLastUpdate, _cachedData);
        }
    }
}