using ChoiceVSharedApiModels.Companys;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ICompanyApi
{
    [Get("/api/v1/company")]
    Task<ApiResponse<List<CompanyApiModel>>> GetAllAsync(); 
}