using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Extensions;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class FileEntryDbModel
{
    public FileEntryDbModel() {}

    public FileEntryDbModel(Ulid supportfileId, string content, Ulid accessId)
    {
        SupportfileId = supportfileId;
        Content = content;
        CreatedByAccessId = accessId;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid SupportfileId { get; set; }
    public string Content { get; set; }
    public Ulid CreatedByAccessId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public string ModifiedAtLocal(string timeZoneId = "Europe/Berlin") => ModifiedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    
    public bool Deleted { get; set; } = false;
    
    // Navigation Properties
    public AccessDbModel CreatorAccessModel { get; set; }
    public FileDbModel FileDbModel { get; set; }
    public List<FileUploadDbModel> FileUploads { get; set; } = [];
}