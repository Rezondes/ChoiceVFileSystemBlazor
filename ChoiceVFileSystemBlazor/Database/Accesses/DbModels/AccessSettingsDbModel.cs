namespace ChoiceVFileSystemBlazor.Database.Accesses.DbModels;

public class AccessSettingsDbModel
{
    public AccessSettingsDbModel() {}

    public AccessSettingsDbModel(Ulid accessId)
    {
        AccessId = accessId;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid AccessId { get; set; }

    public string TimeZone { get; set; } = "Europe/Berlin";
    public bool IsDarkMode { get; set; } = true;
    public bool IsNavbarExpanded { get; set; } = true;
    
    // Navigation Properties
    public AccessDbModel AccessModel { get; set; }
}