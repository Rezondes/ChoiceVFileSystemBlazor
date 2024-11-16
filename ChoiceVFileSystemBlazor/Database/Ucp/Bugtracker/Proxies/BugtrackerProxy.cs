using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Model;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Services.Discord;
using ChoiceVFileSystemBlazor.Services.Vikunja;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies;

public class BugtrackerProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, DiscordBotService discordBotService, VikunjaClientService vikunjaClientService) : IBugtrackerProxy
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

        var allScpBugs = await vikunjaClientService.Client.GetAllTasksInProjectAsync(vikunjaClientService.ScpBugsProjectId);
        var allChoiceVBugs = await vikunjaClientService.Client.GetAllTasksInProjectAsync(vikunjaClientService.ChoiceVBugsProjectId);
        
        var data = new List<BugTaskModel>();
        foreach (var bug in allBugs)
        {
            var discordName = discordBotService.GetUsername(bug.DiscordId);
            var status = await bug.GetStatus(vikunjaClientService, allScpBugs, allChoiceVBugs);

            data.Add(new BugTaskModel(bug.Id, bug.DiscordId, discordName ?? "Unknown User", bug.BugTaskId, bug.BugTaskName, status));
        }
        
        return data;
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