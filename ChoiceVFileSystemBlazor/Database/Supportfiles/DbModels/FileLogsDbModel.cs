using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;
using ChoiceVFileSystemBlazor.Extensions;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class FileLogsDbModel
{
    public FileLogsDbModel() {}

    public FileLogsDbModel(Ulid supportfileId, FileLogTypeEnum type, Ulid accessId, string content)
    {
        SupportfileId = supportfileId;
        Type = type;
        AccessId = accessId;
        Content = content;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid SupportfileId { get; set; }
    public FileLogTypeEnum Type { get; set; }
    public Ulid AccessId { get; set; }
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
        
    // Navigation Properties
    public AccessDbModel AccessModel { get; set; }
    public FileDbModel FileDbModel { get; set; }
}