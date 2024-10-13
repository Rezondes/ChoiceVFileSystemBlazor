namespace ChoiceVFileSystemBlazor.Services.DiscordGuildMembers;

public class DiscordGuildMembersBackgroundService(DiscordGuildMembersCachedService cachedService, ILogger<DiscordGuildMembersBackgroundService> logger) : BackgroundService
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

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}