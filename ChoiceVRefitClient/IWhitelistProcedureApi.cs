using ChoiceVSharedApiModels.Whitelist;
using Refit;

namespace ChoiceVRefitClient;

public interface IWhitelistProcedureApi : IBaseApiInterface
{
    [Get("/api/v1/procedure/overview")]
    Task<ApiResponse<List<WhitelistProcedureApiModel>>> GetAllAsync(CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/procedure?procedureId={procedureId}")]
    Task<ApiResponse<WhitelistProcedureApiModel?>> GetByProcedureIdAsync(int procedureId, CancellationToken cancellationToken = default); 
    
    [Put("/api/v1/procedure/blocked?procedureId={procedureId}&state={state}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<string>> SetBlockedStateAsync(int procedureId, bool state, CancellationToken cancellationToken = default); 
}