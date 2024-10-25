using ChoiceVFileSystemBlazor.Models;
using ChoiceVFileSystemBlazor.Services;
using ChoiceVFileSystemBlazor.Services.DiscordGuildMembers;
using ChoiceVRefitClient;
using MudBlazor;

namespace ChoiceVFileSystemBlazor.Helper;

public static class AccountHelper
{
    public static async Task<bool> OpenAddAccountDialog(
        DiscordGuildMembersCachedService discordGuildMembersCachedService,
        DiscordService discordService,
        IDialogService dialogService, 
        ISnackbar snackbar, 
        IAccountApi accountApi,
        PageLoadingService loadingService,
        string socialClubName = "", 
        string discordId = ""
    )
    {
        const string discordInputLabel = "DiscordId";
        const string discordInputPlaceholder = "SuperKuhlerDiscordId";
        
        var discordInputModel = new InputModel(
            InputTypes.Text,
            discordInputLabel,
            discordId,
            discordInputPlaceholder
        );
        
        var (lastTrySuccess, lastTry, cachedLastUpdate, cachedData) = await discordGuildMembersCachedService.GetCachedData();
        if (lastTrySuccess.HasValue && lastTrySuccess.Value && cachedData is not null)
        {
            var selectOptions = cachedData
                .OrderBy(x => x.Username)
                .Select(discordGuildUser => 
                    new InputOptionModel(discordGuildUser.DiscordId.ToString(), discordGuildUser.Username))
                .ToList();

            discordInputModel = new InputModel(
                InputTypes.Select,
                discordInputLabel,
                discordId,
                discordInputPlaceholder,
                selectOptions
            );
        }
        
        var inputs = new List<InputModel>
        {
            new(
                InputTypes.Text,
                "SocialClubName",
                socialClubName,
                "SuperKuhlerSocialClubName"
            ),
            discordInputModel
        };

        var dialogData = await dialogService.OpenDialog(
            "Spieler whitelisten", 
            "Bist du dir sicher, dass du diesen Spieler whitelisten willst?", 
            "Spieler whitelisten",
            inputs);
        if (dialogData is null) return false;
        
        var (validatedSocialClubName, parsedSocialClubName) = dialogData[0].ValidateInput<string>();
        if (!validatedSocialClubName)
        {
            snackbar.Add("Du hast keinen validen SocialClubNamen eingegeben!", Severity.Error);
            return false;
        }
        
        var (validatedDiscordId, parsedDiscordId) = dialogData[1].ValidateInput<string>();
        if (!validatedDiscordId)
        {
            snackbar.Add("Du hast keine valide DiscordId eingegeben!", Severity.Error);
            return false;
        }
        
        var discordIdValidated = await discordService.ValidateDiscordId(parsedDiscordId!);
        if (!discordIdValidated)
        {
            snackbar.Add("Du hast keine valide DiscordId!", Severity.Error);
            return false;
        }
        
        loadingService.StartLoading();
        var response = await accountApi.AddAccountAsync(parsedSocialClubName!, parsedDiscordId!);
        if (!response.IsSuccessStatusCode)
        {
            snackbar.Add(response.Error.Content, Severity.Error);
            loadingService.StopLoading();
            return false;
        }

        var newAccount = response.Content;
        
        snackbar.Add($"Spieler gewhitelisted! " +
                     $"Id: {newAccount.Id}, " +
                     $"DiscordId: {newAccount.DiscordId}, " +
                     $"SocialClubName: {newAccount.SocialClubName}, " +
                     $"Status: {newAccount.State}");
        
        loadingService.StopLoading();
        
        return true;
    }
}