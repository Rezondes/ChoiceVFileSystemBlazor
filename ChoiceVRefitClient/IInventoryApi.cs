using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Inventory;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IInventoryApi
{
    [Get("/v1/inventory/{characterId}")]
    Task<ApiResponse<InventoryModel>> GetByCharacterIdAsync(int characterId); 
    
}