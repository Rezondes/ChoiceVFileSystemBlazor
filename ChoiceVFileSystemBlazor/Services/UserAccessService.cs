using System.Net.Sockets;
using System.Security.Claims;
using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.Ranks.Proxies.Intefaces;
using ChoiceVFileSystemBlazor.Services.Discord;
using ChoiceVRefitClient;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChoiceVFileSystemBlazor.Services;

public class UserAccessService(
    IAccountApi accountApi, IAccessProxy accessProxy, 
    IRankProxy rankProxy, IDiscordRolesProxy discordRolesProxy, 
    AuthenticationStateProvider authenticationStateProvider)
{
    private AccessDbModel? _userAccess;
    private List<RightEnum> _userRights = [];

    public static string? GetDiscordUserId(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    
    public static string? GetDiscordUserName(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
    }
    
    public static bool HasClaims(ClaimsPrincipal user)
    {
        var discordId = GetDiscordUserId(user);
        var discordName = GetDiscordUserName(user);
        
        return discordId is not null && discordName is not null;
    }

    public static async Task<bool> IsDiscordIdValid(ClaimsPrincipal user, DiscordBotService discordBotService)
    {
        var discordId = GetDiscordUserId(user);
        if (discordId is null) return false;
        var isValid = await discordBotService.ValidateDiscordId(discordId);
        return isValid;
    }
    
    public async Task InitializeUserAsync(ClaimsPrincipal user)
    {
        var discordId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var discordName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var roles = user.Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();

        if (discordId is null || discordName is null) throw new Exception("No Claims");

        var accessDbModel = await accessProxy.GetWithSettingsAsync(discordId);
        if (accessDbModel is null)
        {
            var newAccess = await CreateNewAccessAsync(discordId, discordName);

            _userAccess = newAccess;

            await CheckUpdateRankByDiscordRolesAsync(roles);

            if (_userAccess.Settings is null)
            {
                _userAccess.Settings = await accessProxy.AddSettingsAsync(_userAccess);
            }
        }
        else
        {
            if (accessDbModel.Settings is null)
            {
                accessDbModel.Settings = await accessProxy.AddSettingsAsync(accessDbModel);
            }

            if (accessDbModel.AccountId == -1)
            {
                try
                {
                    var accountResponse = await accountApi.GetByDiscordIdAsync(discordId);
                    if (accountResponse.IsSuccessStatusCode)
                    {
                        var account = accountResponse.Content;
                        
                        accessDbModel.AccountId = account.Id;
                        await accessProxy.UpdateAccountIdAsync(accessDbModel.Id, account.Id, Ulid.Empty);
                    }
                }
                catch (HttpRequestException) { }
            }
            
            _userAccess = accessDbModel;
        }

        var rightToRankDbModels = await rankProxy.GetAllRightsAsync(_userAccess.Rank);
        _userRights = rightToRankDbModels.Select(x => x.Right).ToList();
    }

    public async Task ReloadUserAccessAsync()
    {
        var accessDbModel = await accessProxy.GetWithSettingsAsync(_userAccess.DiscordId);

        _userAccess = accessDbModel ?? throw new Exception("No Access");
        
        var rightToRankDbModels = await rankProxy.GetAllRightsAsync(_userAccess.Rank);
        _userRights = rightToRankDbModels.Select(x => x.Right).ToList();
    }
    
    private async Task<AccessDbModel> CreateNewAccessAsync(string discordId, string discordName)
    {
        AccessDbModel? newAccessModel;
        try
        {
            var accountResponse = await accountApi.GetByDiscordIdAsync(discordId);
            if (accountResponse.IsSuccessStatusCode)
            {
                var account = accountResponse.Content;
                newAccessModel = new AccessDbModel(account.Id, account.DiscordId, account.Name);
            }
            else
            {
                newAccessModel = new AccessDbModel(-1, discordId, discordName);
            }
        }
        catch (HttpRequestException ex)
        {
            newAccessModel = new AccessDbModel(-1, discordId, discordName);
        }

        // sometimes blazor will trigger this event two times so the second time will fail
        // fuck this shit and check if its already there
        var addResponse = await accessProxy.AddAccessModelAsync(newAccessModel);
        if (!addResponse)
        {
            var accountResponse = await accessProxy.GetAsync(discordId);
            if (accountResponse is null)
            {
                throw new Exception(
                    $"Unable to add new Access: " +
                    $"DiscordId {newAccessModel.DiscordId} | " +
                    $"AccountId {newAccessModel.AccountId} | " +
                    $"AccountName {newAccessModel.Name}"
                );
            }
        }

        return newAccessModel;
    }
    
    private async Task CheckUpdateRankByDiscordRolesAsync(List<string> roles)
    {
        if (roles.Count <= 0) return;

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

        if (highestRank <= _userAccess.Rank) return;

        var updateRankResponse = await accessProxy.UpdateRankAsync(_userAccess.Id, highestRank, Ulid.Empty);
        if (!updateRankResponse)
        {
            Console.WriteLine($"Unable to update rank: Id: {_userAccess.Id} | Rank: {highestRank}");
        }
        _userAccess.Rank = highestRank;
    }
    
    public async Task<AccessDbModel> GetUserAccess() {
        if (_userAccess is not null)
        {
            if (_userAccess.Settings is not null)
            {
                return _userAccess;
            }
            _userAccess = await accessProxy.GetWithSettingsAsync(_userAccess.Id);
            return _userAccess;
        }
        
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        await InitializeUserAsync(authState.User);
        return _userAccess;
    }

    public List<RightEnum> GetUserRights() => _userRights;

    public bool HasRight(RightEnum right) 
    {
        return HasRightAsync(right).GetAwaiter().GetResult();
    }
    
    public async Task<bool> HasRightAsync(RightEnum right) 
    {
        if (_userAccess is null)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            await InitializeUserAsync(authState.User);
        }
        
        return right == RightEnum.None || _userAccess.Rank == RankEnum.Admin || _userRights.Contains(right);   
    }

    public bool HasAnyRights(List<RightEnum> rights)
    {
        return HasAnyRightsAsync(rights).GetAwaiter().GetResult();
    }
    
    public async Task<bool> HasAnyRightsAsync(List<RightEnum> rights)
    {
        if (_userAccess is null)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            await InitializeUserAsync(authState.User);
        }
        
        return _userAccess.Rank == RankEnum.Admin || _userRights.Any(rights.Contains);
    }
}