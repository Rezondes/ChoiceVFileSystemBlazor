using ChoiceVFileSystemBlazor.Components._Shared;
using ChoiceVFileSystemBlazor.Models;
using MudBlazor;

namespace ChoiceVFileSystemBlazor.Helper;

public static class LogContentDialogHelper
{
    public static async Task OpenDialogAsync(this IDialogService dialogService, Ulid logId, DateTime date, string content)
    {
        var parameter = new DialogParameters<LogContentDialog>
        {
            { x => x.LogId, logId },
            { x => x.Date, date },
            { x => x.Content, content },
        };

        await dialogService.ShowAsync<LogContentDialog>(string.Empty, parameter, new DialogOptions { FullWidth = true });
    }
}