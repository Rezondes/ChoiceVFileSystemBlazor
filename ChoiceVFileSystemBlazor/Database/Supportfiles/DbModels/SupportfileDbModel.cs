using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileDbModel
{
    public SupportfileDbModel()
    {
    }   
    
    public SupportfileDbModel(Ulid createdByAccessId)
    {
        CreatedByAccessId = createdByAccessId;
    }   

    public SupportfileDbModel(
        string title, string description, Ulid createdByAccessId, 
        FileStatusEnum status, RankEnum minRank)
    {
        Id = Ulid.NewUlid();
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        CreatedByAccessId = createdByAccessId;
        Status = status;
        MinRank = minRank;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();

    [Required]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    public string Title { get; set; }
    [Required]
    [MaxLength(150, ErrorMessage = "Description must be at most 150 characters long.")]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    
    public Ulid CreatedByAccessId { get; set; }
    [Required]
    public FileStatusEnum Status { get; set; }

    [Required] 
    public RankEnum MinRank { get; set; } = RankEnum.Rank1;

    public bool Deleted { get; set; } = false;
    
    // Navigation Properties
    public AccessDbModel CreatorAccessModel { get; set; }
    
    public List<SupportfileCharacterEntryDbModel> CharacterEntrys { get; set; }
    public List<SupportfileEntryDbModel> Entrys { get; set; }
    public List<SupportfileLogsDbModel> Logs { get; set; }
}