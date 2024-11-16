using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Models;
using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Services.Discord;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Proxies;

public class MessageProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, DiscordBotService discordBotService) : IMessageProxy
{
    public async Task<List<MessageToDiscordIdDbModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.MessageToDiscordIdDbModels.ToListAsync(cancellationToken);
    }

    public async Task<List<ChatForDiscordIdModel>> GetAllChatsAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var allMessages = await dbContext.MessageToDiscordIdDbModels.AsNoTracking().ToListAsync(cancellationToken);
        
        var chats = allMessages
            .GroupBy(message => message.ToDiscordId)
            .Select(group =>
            {
                var username = discordBotService.GetUsername(group.Key);
                
                return new ChatForDiscordIdModel(
                    group.Key,
                    username ?? "Unknown User",
                    group.Count(),
                    group.OrderByDescending(message => message.Timestamp).FirstOrDefault()?.Timestamp ?? DateTime.MinValue
                );
            })
            .OrderBy(chat => chat.LastMessageSent)
            .ToList();

        return chats;
    }

    public async Task<List<MessageToDiscordIdDbModel>> GetAllForDiscordIdAsync(string discordId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.MessageToDiscordIdDbModels.Where(x => x.ToDiscordId == discordId).ToListAsync(cancellationToken);
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

    public async Task<bool> HasNewMessagesAsync(string discordId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return dbContext.MessageToDiscordIdDbModels.AsNoTracking().Where(x => x.ToDiscordId == discordId).Any(x => !x.IsReadByUser);
    }
    
    public async Task UpdateToUserReadedAsync(IEnumerable<Ulid> messages)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var messageList = await dbContext.MessageToDiscordIdDbModels
            .Where(x => messages.Contains(x.Id))
            .ToListAsync();

        foreach (var message in messageList)
        {
            message.IsReadByUser = true;
        }
        
        await dbContext.SaveChangesAsync();
    }
}