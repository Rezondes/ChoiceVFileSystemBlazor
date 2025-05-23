﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class FileDbModel
{
    public FileDbModel()
    {
    }   
    
    public FileDbModel(Ulid createdByAccessId, FileTypeEnum type)
    {
        CreatedByAccessId = createdByAccessId;
        Type = type;
    }   

    public FileDbModel(
        string title, string description, Ulid createdByAccessId, 
        FileStatusEnum status, RankEnum minRank, FileTypeEnum type)
    {
        Id = Ulid.NewUlid();
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        CreatedByAccessId = createdByAccessId;
        Status = status;
        MinRank = minRank;
        Type = type;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();

    public string? DisplayId { get; set; }
    
    public FileTypeEnum Type { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    public string Title { get; set; }
    [Required]
    [MaxLength(150, ErrorMessage = "Description must be at most 150 characters long.")]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    
    public Ulid CreatedByAccessId { get; set; }
    
    [Required] 
    public FileStatusEnum Status { get; set; } = FileStatusEnum.Created;

    [Required] 
    public RankEnum MinRank { get; set; } = RankEnum.SupportTrainee;
    
    public Ulid? CategoryId { get; set; }
    
    public bool Deleted { get; set; } = false;
    
    // Navigation Properties
    public AccessDbModel CreatorAccessModel { get; set; }
    public FileCategoryDbModel? Category { get; set; }
    
    public List<FileCharacterEntryDbModel> CharacterEntrys { get; set; }
    public List<FileEntryDbModel> Entrys { get; set; }
    public List<FileLogsDbModel> Logs { get; set; }
}