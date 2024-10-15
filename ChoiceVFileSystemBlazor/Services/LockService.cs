namespace ChoiceVFileSystemBlazor.Services;

public class LockService
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task<T> LockAsync<T>(Func<Task<T>> criticalAction)
    {
        await _semaphore.WaitAsync();
        try
        {
            return await criticalAction();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}