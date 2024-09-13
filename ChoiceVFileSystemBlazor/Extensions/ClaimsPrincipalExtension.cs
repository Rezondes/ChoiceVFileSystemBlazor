using System.Diagnostics;
using System.Security.Claims;
using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;
using ChoiceVRefitClient;
using ChoiceVSharedApiModels.Accounts;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class ClaimsPrincipalExtension
{
    public static async Task<AccessDbModel?> GetVerifiedUser(this ClaimsPrincipal? user, IAccessProxy accessProxy, IAccountApi accountApi, IDiscordRolesProxy discordRolesProxy)
    {
        if (user is null) throw new ArgumentException("parameter is null", nameof(user));

        AccessDbModel? response = null;

        try
        {
            var discordId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var discordName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var roles = user.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
            
            if (discordId is null || discordName is null) throw new Exception("No Claims");

            var accessResponse = await accessProxy.GetAsync(discordId);
            if (accessResponse is null)
            {
                AccessDbModel? newAccessModel;
                
                var accountResponse = await accountApi.GetByDiscordIdAsync(discordId);
                if (accountResponse.IsSuccessStatusCode)
                {
                    var account = accountResponse.Content;
                    // TODO change back on Api ready
                    // newAccessModel = new AccessDbModel(account.Id, account.DiscordId, account.Name);
                    newAccessModel = new AccessDbModel(-1, discordId, discordName);
                }
                else
                {
                    newAccessModel = new AccessDbModel(-1, discordId, discordName);
                }
                
                var addResponse = await accessProxy.AddAccessModelAsync(newAccessModel);
                if (!addResponse)
                    throw new Exception($"Unable to add new Access: DiscordId {newAccessModel.DiscordId} | AccountId {newAccessModel.AccountId} | AccountName {newAccessModel.Name}");
                
                response = newAccessModel;
            
                if (response.AccountId >= 0)
                {
                    // Set Highest DiscordRole
                    var discordRoleDbModels = await discordRolesProxy.GetAllAsync();
                    var highestRank = RankEnum.None;
                    foreach (var role in roles)
                    {
                        if (!ulong.TryParse(role, out var discordRoleId)) continue;

                        var discordRoleDbModel = discordRoleDbModels.FirstOrDefault(x => x.DiscordRoleId == discordRoleId);
                        if (discordRoleDbModel is null) continue;

                        if (discordRoleDbModel.AutomaticRank > highestRank) 
                            highestRank = discordRoleDbModel.AutomaticRank;
                    }

                    if (highestRank > response.Rank)
                    {
                        var updateRankResponse = await accessProxy.UpdateRankAsync(response.Id, highestRank, Ulid.Empty);
                        if (!updateRankResponse)
                        {
                            Console.WriteLine($"Unable to update rank: Id: {response.Id} | Rank: {highestRank}");
                        }
                        response.Rank = highestRank;
                    }
                }
            }
            else
            {
                response = accessResponse;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return response;
    }
}