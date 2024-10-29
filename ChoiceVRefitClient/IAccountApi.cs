using ChoiceVSharedApiModels.Accounts;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IAccountApi : IBaseApiInterface
{
    [Get("/api/v1/account")]
    Task<ApiResponse<List<AccountApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/account?accountId={accountId}")]
    Task<ApiResponse<AccountApiModel?>> GetByIdAsync(int accountId);
    
    [Get("/api/v1/account?discordId={discordId}")]
    Task<ApiResponse<AccountApiModel?>> GetByDiscordIdAsync(string discordId);
    
    [Put("/api/v1/account/ban?accountId={accountId}&message={message}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<AccountBanResultApiModel>> BanAccountByAccountIdAsync(int accountId, string message);
    
    [Put("/api/v1/account/unban?accountId={accountId}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<AccountUnbanResultApiModel>> UnbanAccountByAccountIdAsync(int accountId);
    
    [Put("/api/v1/account/kick?accountId={accountId}&message={message}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<AccountKickResultApiModel>> KickAccountByAccountIdAsync(int accountId, string message);
    
    [Post("/api/v1/account/add?socialClubName={socialClubName}&discordId={discordId}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<AccountApiModel>> AddAccountAsync(string socialClubName, string discordId);
    
    [Put("/api/v1/account/lightmode/remove?accountId={accountId}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<AccountUnbanResultApiModel>> RemoveLightmodeAsync(int accountId);
}