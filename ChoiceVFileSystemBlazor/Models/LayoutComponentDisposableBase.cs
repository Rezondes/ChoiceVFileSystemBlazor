using Microsoft.AspNetCore.Components;

namespace ChoiceVFileSystemBlazor.Models;

public abstract class LayoutComponentDisposableBase : LayoutComponentBase, IDisposable
{
    private bool _disposed = false;

    public virtual void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;
    }

    ~LayoutComponentDisposableBase()
    {
        Dispose(disposing: false);
    }
}