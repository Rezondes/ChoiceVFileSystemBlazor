using System.Text.Json;
using ChoiceVApi._Shared;
using ChoiceVSharedApiModels.Accounts;

namespace ChoiceVApi.Account;

public static class AccountController
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
            case "discord":
                var accountByDiscordId = await GetByDiscordIdAsync(data);
            
                response = JsonSerializer.Serialize(accountByDiscordId);
                break;
            default:
                if (string.IsNullOrEmpty(data))
                {
                    var list = await GetAllAccountsAsync();
            
                    response = JsonSerializer.Serialize(list);
                }
                else
                {
                    if (!int.TryParse(data, out var accountId)) throw new Exception("Invalid data");
                    
                    var account = await GetByIdAsync(accountId);
            
                    response = JsonSerializer.Serialize(account);
                }
                break;
        }
        
        return response;
    }
    
    #endregion

    #region GameserverMethodLinks

    // TODO Durch richtige Daten ersetzen
    private static async Task<List<AccountApiModel>> GetAllAccountsAsync()
    {
        return TestDataGenerator.GenerateList<AccountApiModel>(100); 
    }

    // TODO Durch richtige Daten ersetzen
    private static async Task<AccountApiModel> GetByIdAsync(int accountId)
    {
        var account = TestDataGenerator.GenerateSingle<AccountApiModel>();
        account.Id = accountId;
        
        return account;
    }
    
    // TODO Durch richtige Daten ersetzen
    private static async Task<AccountApiModel> GetByDiscordIdAsync(string discordId)
    {
        var account = TestDataGenerator.GenerateSingle<AccountApiModel>();
        account.DiscordId = discordId;
        
        return account;
    }

    #endregion
}