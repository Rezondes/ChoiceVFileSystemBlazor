using ChoiceVFileSystemBlazor.Database.Accesses.Enums;
using ChoiceVFileSystemBlazor.Extensions;

namespace ChoiceVFileSystemBlazor.Database.Accesses.DbModels;

public class AccessLogsDbModel
{
    public AccessLogsDbModel() {}

    public AccessLogsDbModel(Ulid supportfileId, AccessLogTypeEnum type, Ulid accessId, string content)
    {
        TargetAccessId = supportfileId;
        Type = type;
        AccessId = accessId;
        Content = content;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid TargetAccessId { get; set; }
    public AccessLogTypeEnum Type { get; set; }
    public Ulid AccessId { get; set; }
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    
    // Navigation Properties
    public AccessDbModel TargetAccessModel { get; set; }
    public AccessDbModel AccessModel { get; set; }
}