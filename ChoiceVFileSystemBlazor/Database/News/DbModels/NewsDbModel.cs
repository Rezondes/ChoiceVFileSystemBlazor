using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Extensions;

namespace ChoiceVFileSystemBlazor.Database.News.DbModels;

public class NewsDbModel
{
    public NewsDbModel(){}

    public NewsDbModel(string title, List<string> content, Ulid creatorId)
    {
        Title = title;
        Content = content;
        CreatorId = creatorId;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Title { get; set; }
    public string ContentJson { get; set; } 
    
    [NotMapped]
    public List<string> Content
    {
        get => string.IsNullOrEmpty(ContentJson) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(ContentJson)!;
        set => ContentJson = JsonSerializer.Serialize(value);
    }
    [NotMapped]
    public string ContentString => string.Join("\n", Content);
    
    public Ulid CreatorId { get; set; } 
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string CreatedLocal(string timeZoneId = "Europe/Berlin") => Created.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    

    // Navigation Properties
    public AccessDbModel Creator { get; set; }
}