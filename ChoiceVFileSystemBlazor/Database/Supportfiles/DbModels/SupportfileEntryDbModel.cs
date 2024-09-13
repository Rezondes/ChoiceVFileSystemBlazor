using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileEntryDbModel
{
    public SupportfileEntryDbModel() {}

    public SupportfileEntryDbModel(Ulid supportfileId, string content, Ulid accessId)
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
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public bool Deleted { get; set; } = false;
    
    // Navigation Properties
    public AccessDbModel CreatorAccessModel { get; set; }
    public List<SupportfileFileUploadDbModel> FileUploads { get; set; } = [];
}