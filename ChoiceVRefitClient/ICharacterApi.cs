using System.Reflection.PortableExecutable;
using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Characters;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ICharacterApi
{
    [Get("/api/v1/character")]
    Task<ApiResponse<List<CharacterApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/character?characterId={characterId}")]
    Task<ApiResponse<CharacterApiModel?>> GetByCharacterIdAsync(int characterId); 
    
    [Get("/api/v1/character?accountId={accountId}")] 
    Task<ApiResponse<List<CharacterApiModel>>> GetByAccountIdAsync(int accountId); 
    
    [Get("/api/v1/character/live")]
    Task<ApiResponse<List<CharacterApiModel>>> GetAllLiveAsync(); 
    
    [Put("/api/v1/character/dimension?characterId={characterId}&dimension={dimension}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<CharacterChangeDimensionResultApiModel>> ChangeDimensionAsync(int characterId, int dimension);
    
    [Put("/api/v1/character/dead?characterId={characterId}&state={state}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<CharacterSetPermadeathActivatedResultApiModel>> SetPermadeathActivatedAsync(int characterId, bool state);
    
    [Put("/api/v1/character/crimeflag?characterId={characterId}&state={state}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<CharacterSetCrimeFlagActiveResultApiModel>> SetCrimeFlagActiveAsync(int characterId, bool state);
}