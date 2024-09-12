using System.Text.Json;
using ChoiceVApi._Shared;
using ChoiceVSharedApiModels.Characters;

namespace ChoiceVApi.Character;

public class CharacterController
{
    private static Random _random = new Random();

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
            case "account":
                if (!int.TryParse(data, out var accountId)) throw new Exception("Invalid data");
                    
                var account = await GetAllCharactersByAccountId(accountId);
            
                response = JsonSerializer.Serialize(account);
                break;
            
            default:
                if (string.IsNullOrEmpty(data))
                {
                    var list = await GetAllCharactersAsync();
            
                    response = JsonSerializer.Serialize(list);
                }
                else
                {
                    if (!int.TryParse(data, out var characterId)) throw new Exception("Invalid data");
                    
                    var character = await GetCharacterById(characterId);
            
                    response = JsonSerializer.Serialize(character);
                }
                break;
        }
        
        return response;
    }

    #endregion
    
    #region GameserverMethodLinks

    // TODO Durch richtige Daten ersetzen
    private static async Task<List<CharacterModel>> GetAllCharactersAsync()
    {
        await Task.Delay(_random.Next(1000, 2000));
        return TestDataGenerator.GenerateList<CharacterModel>(100); 
    }

    // TODO Durch richtige Daten ersetzen
    private static async Task<List<CharacterModel>> GetAllCharactersByAccountId(int accountId)
    {
        await Task.Delay(_random.Next(1000, 2000));
        return TestDataGenerator.GenerateList<CharacterModel>(3); 
    }

    // TODO Durch richtige Daten ersetzen
    private static async Task<CharacterModel> GetCharacterById(int characterId)
    {
        await Task.Delay(_random.Next(1000, 2000));
        return TestDataGenerator.GenerateSingle<CharacterModel>();
    }

    #endregion
}