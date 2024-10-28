using System.Reflection.Emit;
using ChoiceVSharedApiModels.SupportKeyInfo;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ISupportKeyInfoApi
{
    [Get("/api/v1/supportkeyinfo")]
    Task<ApiResponse<List<SupportKeyInfoApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/supportkeyinfo?supportKeyInfoId={supportKeyInfoId}")]
    Task<ApiResponse<SupportKeyInfoApiModel>> GetByIdAsync(int supportKeyInfoId); 
}