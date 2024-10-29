using ChoiceVSharedApiModels.Companys;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ICompanyApi : IBaseApiInterface
{
    [Get("/api/v1/company")]
    Task<ApiResponse<List<CompanyApiModel>>> GetAllAsync(CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/company?companyId={companyId}")]
    Task<ApiResponse<CompanyApiModel?>> GetByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/company?characterId={characterId}")]
    Task<ApiResponse<List<CompanyApiModel>>> GetAllByCharacterIdAsync(int characterId, CancellationToken cancellationToken = default); 
}