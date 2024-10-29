using System.Reflection.PortableExecutable;
using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Characters;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ICharacterApi
{
    [Get("/api/v1/character")]
    Task<ApiResponse<List<CharacterApiModel>>> GetAllAsync(CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/character?characterId={characterId}")]
    Task<ApiResponse<CharacterApiModel?>> GetByCharacterIdAsync(int characterId, CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/character?accountId={accountId}")] 
    Task<ApiResponse<List<CharacterApiModel>>> GetByAccountIdAsync(int accountId, CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/character/live")]
    Task<ApiResponse<List<CharacterApiModel>>> GetAllLiveAsync(CancellationToken cancellationToken = default); 
    
    [Put("/api/v1/character/dimension?characterId={characterId}&dimension={dimension}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<CharacterChangeDimensionResultApiModel>> ChangeDimensionAsync(int characterId, int dimension, CancellationToken cancellationToken = default);
    
    [Put("/api/v1/character/dead?characterId={characterId}&state={state}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<CharacterSetPermadeathActivatedResultApiModel>> SetPermadeathActivatedAsync(int characterId, bool state, CancellationToken cancellationToken = default);
    
    [Put("/api/v1/character/crimeflag?characterId={characterId}&state={state}")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<CharacterSetCrimeFlagActiveResultApiModel>> SetCrimeFlagActiveAsync(int characterId, bool state, CancellationToken cancellationToken = default);
}