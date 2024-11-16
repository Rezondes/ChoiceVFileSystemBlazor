using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Model;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Services.Discord;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies;

public class BugtrackerProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, DiscordBotService discordBotService) : IBugtrackerProxy
{
    public async Task<List<DiscordIdToBugTaskIdDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.DiscordIdToBugTaskIdDbModels.AsNoTracking().ToListAsync();
    }

    public async Task<List<BugTaskModel>> GetAllBugsAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var allBugs = await dbContext.DiscordIdToBugTaskIdDbModels.AsNoTracking().ToListAsync(cancellationToken);

        var bugTasksModels = allBugs.Select(x =>
        {
            var discordName = discordBotService.GetUsername(x.DiscordId);

            return new BugTaskModel(
                x.Id, 
                x.DiscordId, 
                discordName ?? "Unknown User",
                x.BugTaskId, 
                x.BugTaskName
            );
        })
        .ToList();

        return bugTasksModels;
    }

    public async Task<List<DiscordIdToBugTaskIdDbModel>> GetAllForDiscordIdAsync(string discordId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.DiscordIdToBugTaskIdDbModels.AsNoTracking().Where(x => x.DiscordId == discordId).ToListAsync();
    }

    public async Task<DiscordIdToBugTaskIdDbModel> AddAsync(DiscordIdToBugTaskIdDbModel model)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Set<DiscordIdToBugTaskIdDbModel>().AnyAsync(m => m.DiscordId == model.DiscordId && m.BugTaskId == model.BugTaskId))
            throw new InvalidOperationException("Ein Eintrag mit dieser ID existiert bereits.");

        dbContext.Set<DiscordIdToBugTaskIdDbModel>().Add(model);
        await dbContext.SaveChangesAsync();
        
        return model;
    }

    public async Task RemoveAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var model = await dbContext.Set<DiscordIdToBugTaskIdDbModel>().FindAsync(id);
        if (model == null)
            throw new KeyNotFoundException("Eintrag nicht gefunden.");

        dbContext.Set<DiscordIdToBugTaskIdDbModel>().Remove(model);
        await dbContext.SaveChangesAsync();
    }
}