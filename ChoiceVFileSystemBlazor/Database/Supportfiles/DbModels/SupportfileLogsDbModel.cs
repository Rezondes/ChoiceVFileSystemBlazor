using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;
using ChoiceVFileSystemBlazor.Extensions;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileLogsDbModel
{
    public SupportfileLogsDbModel() {}

    public SupportfileLogsDbModel(Ulid supportfileId, SupportfileLogTypeEnum type, Ulid accessId, string content)
    {
        SupportfileId = supportfileId;
        Type = type;
        AccessId = accessId;
        Content = content;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid SupportfileId { get; set; }
    public SupportfileLogTypeEnum Type { get; set; }
    public Ulid AccessId { get; set; }
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
        
    // Navigation Properties
    public AccessDbModel AccessModel { get; set; }
}