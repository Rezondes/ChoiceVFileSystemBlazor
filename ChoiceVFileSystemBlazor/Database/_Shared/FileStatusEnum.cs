namespace ChoiceVFileSystemBlazor.Database._Shared;

public enum FileStatusEnum
{
    Created = 0,
    WorkInProgress = 50,
    Closed = 100,
}

public static class FileStatusEnumExtensions
{
    public static string GetDisplayText(this FileStatusEnum status)
    {
        return status switch
        {
            FileStatusEnum.Created => "Erstellt",
            FileStatusEnum.WorkInProgress => "In Bearbeitung",
            FileStatusEnum.Closed => "Abgeschlossen",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
