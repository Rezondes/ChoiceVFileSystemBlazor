using ChoiceVFileSystemBlazor.Services._Shared;
using Microsoft.Extensions.Options;
using Refit;

namespace ChoiceVFileSystemBlazor.Services.Vikunja;

public class VikunjaClientService
{
    public readonly IVikunjaClient Client;
    
    public readonly int VikunjaApiUserId;
    public readonly int ScpBugsProjectId;
    public readonly int ScpBugsViewId;
    public readonly int ChoiceVBugsProjectId;
    public readonly int ChoiceVBugsViewId;

    public VikunjaClientService(IOptions<VikunjaSettings> settings)
    {
        VikunjaApiUserId = settings.Value.VikunjaApiUserId;
        ScpBugsProjectId = settings.Value.ScpBugsProjectId;
        ScpBugsViewId = settings.Value.ScpBugsViewId;
        ChoiceVBugsProjectId = settings.Value.ChoiceVBugsProjectId;
        ChoiceVBugsViewId = settings.Value.ChoiceVBugsViewId;
        
        var httpClient = new HttpClient(new AuthenticatedHttpClientHandler(settings.Value.ApiKey))
        {
            BaseAddress = new Uri(settings.Value.BaseAddress)
        };
        
        Client = RestService.For<IVikunjaClient>(httpClient);
    }
}


