using ChoiceVFileSystemBlazor.Services._Shared;
using Microsoft.Extensions.Options;
using Refit;

namespace ChoiceVFileSystemBlazor.Services.Vikunja;

public class VikunjaClientService
{
    public readonly IVikunjaClient Client;
    
    public readonly int ScpBugsProjectId;
    public readonly int ChoiceVBugsProjectId;

    public VikunjaClientService(IOptions<VikunjaSettings> settings)
    {
        ScpBugsProjectId = settings.Value.ScpBugsProjectId;
        ChoiceVBugsProjectId = settings.Value.ChoiceVBugsProjectId;
        
        var httpClient = new HttpClient(new AuthenticatedHttpClientHandler(settings.Value.ApiKey))
        {
            BaseAddress = new Uri(settings.Value.BaseAddress)
        };
        
        Client = RestService.For<IVikunjaClient>(httpClient);
    }
}


