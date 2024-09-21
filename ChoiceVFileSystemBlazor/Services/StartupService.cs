using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;

namespace ChoiceVFileSystemBlazor.Services;

public class StartupService(IServiceProvider  serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var accessProxy = scope.ServiceProvider.GetRequiredService<IAccessProxy>();

        await accessProxy.AddSystemAccessAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}