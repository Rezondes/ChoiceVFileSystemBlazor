namespace ChoiceVFileSystemBlazor.Services.Vikunja;

public class VikunjaSettings
{
    public string BaseAddress { get; set; }
    public string ApiKey { get; set; }
    public int VikunjaApiUserId { get; set; }
    public int ScpBugsProjectId { get; set; }
    public int ScpBugsViewId { get; set; }
    public int ChoiceVBugsProjectId { get; set; }
    public int ChoiceVBugsViewId { get; set; }

    public VikunjaSettings() { }
}