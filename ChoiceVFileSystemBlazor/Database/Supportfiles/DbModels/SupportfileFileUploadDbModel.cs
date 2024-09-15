using System.ComponentModel.DataAnnotations.Schema;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileFileUploadDbModel
{
    public SupportfileFileUploadDbModel() {}

    public SupportfileFileUploadDbModel(Ulid entryId, string fileName, string contentType, byte[] data)
    {
        EntryId = entryId;
        FileName = fileName;
        ContentType = contentType;
        Data = data;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public Ulid EntryId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Data { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    
    [NotMapped]
    public DateTime UploadDateLocal => UploadDate.ToLocalTime();

    public int Size => Data.Length;

    public string SizeText
    {
        get
        {
            var size = Size;

            return size switch
            {
                < 1024 => $"{size} B",
                < 1048576 => $"{size / 1024.0:F2} KB",
                _ => $"{size / 1048576.0:F2} MB"
            };
        }
    }

    // Navigation Properties
    public SupportfileEntryDbModel EntryModel { get; set; }
}