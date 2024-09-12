using System.Reflection.PortableExecutable;
using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Characters;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface ICharacterApi
{
    [Get("/v1/character/")]
    Task<ApiResponse<List<CharacterModel>>> GetAllAsync(); 
    
    [Get("/v1/character/{characterId}")]
    Task<ApiResponse<CharacterModel>> GetByCharacterIdAsync(int characterId); 
    
    [Get("/v1/character/account/{accountId}")]
    Task<ApiResponse<List<CharacterModel>>> GetByAccountIdAsync(int accountId); 
}