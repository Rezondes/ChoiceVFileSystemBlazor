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
}