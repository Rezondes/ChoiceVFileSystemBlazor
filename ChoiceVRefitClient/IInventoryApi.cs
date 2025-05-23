﻿using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Inventory;
using Refit;

namespace ChoiceVRefitClient;

[Headers("Content-Type: application/json", "accept: application/json")]
public interface IInventoryApi : IBaseApiInterface
{
    [Get("/api/v1/inventory?characterId={characterId}")]
    Task<ApiResponse<InventoryApiModel>> GetByCharacterIdAsync(int characterId, CancellationToken cancellationToken = default); 
    
}