using ChoiceVSharedApiModels.Server;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IServerApi : IBaseApiInterface
{
    [Get("/api/v1/server/info")]
    Task<ApiResponse<CurrentServerInfosApiModel>> GetCurrentServerInfosAsync(); 
}