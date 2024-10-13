namespace ChoiceVFileSystemBlazor.Services.Serverinformations;

public class ServerInformationBackgroundService(ServerInformationCachedService cachedService, ILogger<ServerInformationBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await cachedService.FetchDataFromApi();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching data from API.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}