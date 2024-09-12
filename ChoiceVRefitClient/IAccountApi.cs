using ChoiceVSharedApiModels.Accounts;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IAccountApi 
{
    [Get("/v1/account/")]
    Task<ApiResponse<List<AccountModel>>> GetAllAsync(); 
    
    [Get("/v1/account/{accountId}")]
    Task<ApiResponse<AccountModel>> GetByIdAsync(int accountId);
    
    [Get("/v1/account/discord/{discordId}")]
    Task<ApiResponse<AccountModel>> GetByDiscordIdAsync(string discordId);
}