using System.Reflection.PortableExecutable;
using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Characters;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ICharacterApi
{
    [Get("/v1/character")]
    Task<ApiResponse<List<CharacterApiModel>>> GetAllAsync(); 
    
    [Get("/v1/character?characterId={characterId}")]
    Task<ApiResponse<CharacterApiModel?>> GetByCharacterIdAsync(int characterId); 
    
    [Get("/v1/character?accountId={accountId}")]
    Task<ApiResponse<List<CharacterApiModel>>> GetByAccountIdAsync(int accountId); 
}