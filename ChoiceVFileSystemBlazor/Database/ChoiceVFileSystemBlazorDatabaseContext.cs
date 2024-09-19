using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Discord.DbModels;
using ChoiceVFileSystemBlazor.Database.News.DbModels;
using ChoiceVFileSystemBlazor.Database.Ranks.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database;

public class ChoiceVFileSystemBlazorDatabaseContext(DbContextOptions<ChoiceVFileSystemBlazorDatabaseContext> options) : DbContext(options)
{
    #region Access
    public DbSet<AccessDbModel> AccessDbModels { get; set; }
    public DbSet<AccessLogsDbModel> AccessLogsDbModels { get; set; }
    public DbSet<AccessSettingsDbModel> AccessSettingsDbModels { get; set; }
    #endregion
    
    #region Discord
    public DbSet<DiscordRoleDbModel> DiscordRoleDbModels { get; set; }
    public DbSet<DiscordRoleLogsDbModel> DiscordRoleLogsDbModels { get; set; }
    #endregion
    
    #region News
    public DbSet<NewsDbModel> NewsDbModels { get; set; }
    #endregion
    
    #region Ranks
    public DbSet<RightToRankDbModel> RightToRankDbModels { get; set; }
    #endregion
    
    #region Supportfiles 
    public DbSet<SupportfileCharacterEntryDbModel> SupportfileCharacterEntryDbModels { get; set; }
    public DbSet<SupportfileDbModel> SupportfileDbModels { get; set; }
    public DbSet<SupportfileEntryDbModel> SupportfileEntryDbModels { get; set; }
    public DbSet<SupportfileLogsDbModel> SupportfileLogsDbModels  { get; set; }
    public DbSet<SupportfileFileUploadDbModel> SupportfileFileUploadDbModels  { get; set; }
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        #region Access
        
        modelBuilder.Entity<AccessDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<AccessDbModel>()
            .HasOne(a => a.Settings) 
            .WithOne(a => a.AccessModel) 
            .HasForeignKey<AccessSettingsDbModel>(a => a.AccessId) 
            .OnDelete(DeleteBehavior.Cascade); 
        
        modelBuilder.Entity<AccessLogsDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<AccessLogsDbModel>()
            .Property(e => e.TargetAccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<AccessLogsDbModel>()
            .Property(e => e.AccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<AccessLogsDbModel>()
            .HasOne(s => s.AccessModel)
            .WithMany(a => a.CreatedAccessLogs) 
            .HasForeignKey(s => s.AccessId); 
        modelBuilder.Entity<AccessLogsDbModel>()
            .HasOne(s => s.TargetAccessModel)
            .WithMany(a => a.TargetedAccessLogs) 
            .HasForeignKey(s => s.TargetAccessId); 
        
        modelBuilder.Entity<AccessSettingsDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<AccessSettingsDbModel>()
            .Property(e => e.AccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        #endregion
        
        #region Discord
        
        modelBuilder.Entity<DiscordRoleDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        
        modelBuilder.Entity<DiscordRoleLogsDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<DiscordRoleLogsDbModel>()
            .Property(e => e.AccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        
        #endregion
        
        #region News
        
        modelBuilder.Entity<NewsDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<NewsDbModel>()
            .Property(e => e.CreatorId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        
        modelBuilder.Entity<NewsDbModel>()
            .HasOne(s => s.Creator)
            .WithMany(a => a.NewsDbModels) 
            .HasForeignKey(s => s.CreatorId); 
        
        #endregion
        
        #region Ranks

        modelBuilder.Entity<RightToRankDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));

        #endregion
        
        #region Supportfiles

        modelBuilder.Entity<SupportfileCharacterEntryDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileCharacterEntryDbModel>()
            .Property(e => e.SupportfileId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileCharacterEntryDbModel>()
            .HasOne(s => s.Supportfile)
            .WithMany(a => a.CharacterEntrys)
            .HasForeignKey(s => s.SupportfileId);
        
        modelBuilder.Entity<SupportfileDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileDbModel>()
            .Property(e => e.CreatedByAccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileDbModel>()
            .HasOne(s => s.CreatorAccessModel)
            .WithMany(a => a.Supportfiles) 
            .HasForeignKey(s => s.CreatedByAccessId); 

        modelBuilder.Entity<SupportfileLogsDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileLogsDbModel>()
            .Property(e => e.SupportfileId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileLogsDbModel>()
            .Property(e => e.AccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v)); 
        modelBuilder.Entity<SupportfileLogsDbModel>()
            .HasOne(s => s.AccessModel)
            .WithMany(a => a.SupportfileLogs) 
            .HasForeignKey(s => s.AccessId); 
        modelBuilder.Entity<SupportfileLogsDbModel>()
            .HasOne(s => s.SupportfileDbModel)
            .WithMany(a => a.Logs) 
            .HasForeignKey(s => s.SupportfileId); 
        
        modelBuilder.Entity<SupportfileEntryDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileEntryDbModel>()
            .Property(e => e.SupportfileId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileEntryDbModel>()
            .Property(e => e.CreatedByAccessId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileEntryDbModel>()
            .HasOne(s => s.CreatorAccessModel)
            .WithMany(a => a.SupportfileEntrys) 
            .HasForeignKey(s => s.CreatedByAccessId); 
        modelBuilder.Entity<SupportfileEntryDbModel>()
            .HasOne(s => s.SupportfileDbModel)
            .WithMany(a => a.Entrys) 
            .HasForeignKey(s => s.SupportfileId); 
        
        modelBuilder.Entity<SupportfileFileUploadDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileFileUploadDbModel>()
            .Property(e => e.EntryId)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));
        modelBuilder.Entity<SupportfileFileUploadDbModel>()
            .HasOne(s => s.EntryModel)
            .WithMany(a => a.FileUploads) 
            .HasForeignKey(s => s.EntryId); 
        #endregion
    }
}