using System.Text.Json;
using ChoiceVApi._Shared;
using ChoiceVSharedApiModels.Characters;
using ChoiceVSharedApiModels.Inventory;

namespace ChoiceVApi.Inventory;

public class InventoryController
{
    #region ApiHandle

    public static async Task<string> Handle(string httpMethod, string action, string data)
    {
        string response = string.Empty;

        switch (httpMethod)
        {
            case "GET":
                response = await Get(action, data);
                break;
        }
        
        return response;
    }

    private static async Task<string> Get(string action, string data)
    {
        string response;

        switch (action)
        {
            default:
                if (string.IsNullOrEmpty(data))
                {
                    throw new ArgumentNullException(nameof(data));
                }
                else
                {
                    if (!int.TryParse(data, out var characterId)) throw new Exception("Invalid data");
                    
                    var account = await GetByCharacterId(characterId);
            
                    response = JsonSerializer.Serialize(account);
                }
                break;
        }
        
        return response;
    }
    
    #endregion
    
    #region GameserverMethodLinks
    
    // TODO Durch richtige Daten ersetzen
    private static async Task<InventoryApiModel> GetByCharacterId(int characterId)
    {
        return new InventoryApiModel
        {
            CharacterId = characterId,
            Items = TestDataGenerator.GenerateList<InventoryItemApiModel>(25) 
        };
    }
    
    #endregion
}