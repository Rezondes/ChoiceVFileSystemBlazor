namespace ChoiceVFileSystemBlazor.Services.Vikunja;

public class VikunjaSettings
{
    public string BaseAddress { get; set; }
    public string ApiKey { get; set; }
    public int ScpBugsProjectId { get; set; }
    public int ChoiceVBugsProjectId { get; set; }

    public VikunjaSettings() { }
}