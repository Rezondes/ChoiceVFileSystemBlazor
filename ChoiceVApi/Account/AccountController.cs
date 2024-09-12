using System.Text.Json;
using ChoiceVApi._Shared;
using ChoiceVSharedApiModels.Accounts;

namespace ChoiceVApi.Account;

public static class AccountController
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
    private static async Task<List<AccountModel>> GetAllAccountsAsync()
    {
        await Task.Delay(_random.Next(1000, 2000));
        return TestDataGenerator.GenerateList<AccountModel>(100); 
    }

    // TODO Durch richtige Daten ersetzen
    private static async Task<AccountModel> GetByIdAsync(int accountId)
    {
        await Task.Delay(_random.Next(1000, 2000));
        
        var account = TestDataGenerator.GenerateSingle<AccountModel>();
        account.Id = accountId;
        
        return account;
    }
    
    // TODO Durch richtige Daten ersetzen
    private static async Task<AccountModel> GetByDiscordIdAsync(string discordId)
    {
        await Task.Delay(_random.Next(1000, 2000));
        
        var account = TestDataGenerator.GenerateSingle<AccountModel>();
        account.DiscordId = discordId;
        
        return account;
    }

    #endregion
}