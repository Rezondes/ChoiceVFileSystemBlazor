using ChoiceVSharedApiModels.Accounts;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IAccountApi 
{
    [Get("/api/v1/account")]
    Task<ApiResponse<List<AccountApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/account?accountId={accountId}")]
    Task<ApiResponse<AccountApiModel?>> GetByIdAsync(int accountId);
    
    [Get("/api/v1/account?discordId={discordId}")]
    Task<ApiResponse<AccountApiModel?>> GetByDiscordIdAsync(string discordId);
}