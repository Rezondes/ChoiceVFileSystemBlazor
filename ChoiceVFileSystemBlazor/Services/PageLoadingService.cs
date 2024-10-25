namespace ChoiceVFileSystemBlazor.Services;

public class PageLoadingService
{
    public event Action? OnChange;
    private bool _showLoading;

    public bool ShowLoading
    {
        get => _showLoading;
        set
        {
            if (_showLoading == value) return;
            
            _showLoading = value;
            NotifyStateChanged();
        }
    }

    public void StartLoading()
    {
        ShowLoading = true;
    }

    public void StopLoading()
    {
        ShowLoading = false;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}