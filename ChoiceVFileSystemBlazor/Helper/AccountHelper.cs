using ChoiceVFileSystemBlazor.Models;
using ChoiceVFileSystemBlazor.Services;
using ChoiceVFileSystemBlazor.Services.Discord;
using ChoiceVRefitClient;
using MudBlazor;

namespace ChoiceVFileSystemBlazor.Helper;

public static class AccountHelper
{
    public static async Task<bool> OpenAddAccountDialog(
        DiscordBotService discordBotService,
        IDialogService dialogService, 
        ISnackbar snackbar, 
        IAccountApi accountApi,
        PageLoadingService loadingService,
        string socialClubName = "", 
        string discordId = "",
        bool manuelInput = false,
        CancellationToken cancellationToken = default
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
        
        var discordIdValidated = await discordBotService.ValidateDiscordId(parsedDiscordId!);
        if (!discordIdValidated)
        {
            snackbar.Add("Du hast keine valide DiscordId!", Severity.Error);
            return false;
        }
        
        loadingService.StartLoading();
        
        var result = await accountApi.HandleApiRequestAsync(
            async token => await accountApi.AddAccountAsync(parsedSocialClubName!, parsedDiscordId!, token),
            cancellationToken);
        
        if (!result.IsSuccess)
        {
            snackbar.Add(result.Error?.Message ?? "Unknown error", Severity.Error);
            loadingService.StopLoading();
            return false;
        }
        
        var newAccount = result.Data;
        snackbar.Add($"Spieler gewhitelisted! " +
                     $"Id: {newAccount.Id}, " +
                     $"DiscordId: {newAccount.DiscordId}, " +
                     $"SocialClubName: {newAccount.SocialClubName}, " +
                     $"Status: {newAccount.State}");
        
        loadingService.StopLoading();
        
        return true;
    }
}