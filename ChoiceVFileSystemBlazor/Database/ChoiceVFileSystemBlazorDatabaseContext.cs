using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Discord.DbModels;
using ChoiceVFileSystemBlazor.Database.Ranks.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database;

public class ChoiceVFileSystemBlazorDatabaseContext(DbContextOptions<ChoiceVFileSystemBlazorDatabaseContext> options) : DbContext(options)
{
    #region Access
    public DbSet<AccessLogsDbModel> AccessLogsDbModels { get; set; }
    public DbSet<AccessDbModel> AccessDbModels { get; set; }
    #endregion
    
    #region Discord
    public DbSet<DiscordRoleDbModel> DiscordRoleDbModels { get; set; }
    public DbSet<DiscordRoleLogsDbModel> DiscordRoleLogsDbModels { get; set; }
    #endregion
    
    #region Ranks
    public DbSet<RightToRankDbModel> RightToRankDbModels { get; set; }
    #endregion
    
    #region Supportfiles 
    public DbSet<SupportfileDbModel> SupportfileDbModels { get; set; }
    public DbSet<SupportfileEntryDbModel> SupportfileEntryDbModels { get; set; }
    public DbSet<SupportfileLogsDbModel> SupportfileLogsDbModels  { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Access
        
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
        
        modelBuilder.Entity<AccessDbModel>()
            .Property(e => e.Id)
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
        
        #region Ranks

        modelBuilder.Entity<RightToRankDbModel>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v));

        #endregion
        
        #region Supportfiles

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
        
        #endregion
    }
}