using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies;

public class MessageProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IMessageProxy
{
    public async Task<List<MessageToDiscordIdDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.MessageToDiscordIdDbModels.AsNoTracking().ToListAsync();
    }

    public async Task<List<MessageToDiscordIdDbModel>> GetAllForDiscordIdAsync(string discordId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.MessageToDiscordIdDbModels.AsNoTracking().Where(x => x.ToDiscordId == discordId).ToListAsync();
    }

    public async Task<MessageToDiscordIdDbModel> AddAsync(MessageToDiscordIdDbModel model)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (model.IsFromUser)
        {
            model.IsReadByUser = true;
        }
        
        dbContext.Set<MessageToDiscordIdDbModel>().Add(model);
        await dbContext.SaveChangesAsync();
        
        return model;
    }

    public async Task RemoveAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var model = await dbContext.Set<MessageToDiscordIdDbModel>().FindAsync(id);
        if (model == null)
            throw new KeyNotFoundException("Eintrag nicht gefunden.");

        dbContext.Set<MessageToDiscordIdDbModel>().Remove(model);
        await dbContext.SaveChangesAsync();
    }
}