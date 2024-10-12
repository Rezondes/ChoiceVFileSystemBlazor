using ChoiceVSharedApiModels.Whitelist;
using Refit;

namespace ChoiceVRefitClient;

public interface IWhitelistProcedureApi
{
    [Get("/api/v1/procedure/overview")]
    Task<ApiResponse<List<WhitelistProcedureApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/procedure?procedureId={procedureId}")]
    Task<ApiResponse<WhitelistProcedureApiModel?>> GetByProcedureIdAsync(uint procedureId); 
}