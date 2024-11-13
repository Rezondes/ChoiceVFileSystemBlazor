using ChoiceVFileSystemBlazor.Components._Ucp.Bugtracker;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;
using ChoiceVFileSystemBlazor.Extensions;
using ChoiceVFileSystemBlazor.Services.Vikunja;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace ChoiceVFileSystemBlazor.Components._Ucp.Helper;

public static class DiscordIdToBugTaskIdDbModelHelper
{
    public static async Task OpenDialog(this DiscordIdToBugTaskIdDbModel discordIdToBugTaskIdDbModel, VikunjaClientService vikunjaClientService, 
        ISnackbar snackbar, IDialogService dialogService, AuthenticationStateProvider authenticationStateProvider)
    {
        
        var result = await vikunjaClientService.Client.HandleApiRequestAsync(
            async _ => await vikunjaClientService.Client.GetTaskByIdAsync(discordIdToBugTaskIdDbModel.BugTaskId));
        if (!result.IsSuccess)
        {
            snackbar.Add("Der Bug konnte nicht geladen werden.", Severity.Error);
            return;
        }
        
        // var resultAttachments = await VikunjaClientService.Client.HandleApiRequestAsync(
        //     async _ => await VikunjaClientService.Client.GetAllAttachmentsForTaskAsync(discordIdToBugTaskIdDbModel.BugTaskId));
        // if (!resultAttachments.IsSuccess)
        // {
        //     Snackbar.Add("Der Bug konnte nicht geladen werden.", Severity.Error);
        //     return;
        // }
        
        var resultComments = await vikunjaClientService.Client.HandleApiRequestAsync(
            async _ => await vikunjaClientService.Client.GetAllCommentsForTaskAsync(discordIdToBugTaskIdDbModel.BugTaskId));
        if (!resultComments.IsSuccess)
        {
            snackbar.Add("Der Bug konnte nicht geladen werden.", Severity.Error);
            return;
        }
        
        var discordUserName = await authenticationStateProvider.GetDiscordUserNameAsync();
        
        var parameter = new DialogParameters<BugTrackerTaskDialog>
        {
            { x => x.Task, result.Data },
            // { x => x.Attachments, resultAttachments.Data },
            { x => x.Comments, resultComments.Data },
            { x => x.OwnDiscordName, discordUserName },
        };

        await dialogService.ShowAsync<BugTrackerTaskDialog>(
            string.Empty,
            parameter,
            new DialogOptions
            {
                FullWidth = true,
                MaxWidth = MaxWidth.ExtraLarge, 
                CloseOnEscapeKey = true,
                NoHeader = true
            });
    }
}