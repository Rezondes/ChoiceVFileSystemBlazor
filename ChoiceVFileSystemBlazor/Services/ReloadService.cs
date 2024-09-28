namespace ChoiceVFileSystemBlazor.Services;

public class ReloadService
{
    public event Func<Task> OnCustomReload;

    public async Task TriggerCustomReload()
    {
        await OnCustomReload.Invoke();
    }
}