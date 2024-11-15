using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Models;
using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Proxies;

public class MessageProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IMessageProxy
{
    public async Task<List<MessageToDiscordIdDbModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.MessageToDiscordIdDbModels.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<ChatForDiscordIdModel>> GetAllChatsAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var allMessages = await dbContext.MessageToDiscordIdDbModels.AsNoTracking().ToListAsync(cancellationToken);
        
        var chats = allMessages
            .GroupBy(message => message.ToDiscordId)
            .Select(group => new ChatForDiscordIdModel
            {
                DiscordId = group.Key,
                MessageCount = group.Count(),
                LastMessageSent = group.OrderByDescending(message => message.Timestamp).FirstOrDefault()?.Timestamp ?? DateTime.MinValue
            })
            .OrderBy(message => message.LastMessageSent)
            .ToList();

        return chats;
    }

    public async Task<List<MessageToDiscordIdDbModel>> GetAllForDiscordIdAsync(string discordId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.MessageToDiscordIdDbModels.AsNoTracking().Where(x => x.ToDiscordId == discordId).ToListAsync(cancellationToken);
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