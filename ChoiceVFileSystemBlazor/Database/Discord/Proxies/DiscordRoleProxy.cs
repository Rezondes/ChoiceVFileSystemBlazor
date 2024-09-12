using ChoiceVFileSystemBlazor.Database.Discord.DbModels;
using ChoiceVFileSystemBlazor.Database.Discord.DbModels.Partials;
using ChoiceVFileSystemBlazor.Database.Discord.Enums;
using ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Discord.Proxies;

public class DiscordRoleProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, IDiscordRoleLogsProxy discordRoleLogsProxy) : IDiscordRolesProxy
{
    public async Task<List<DiscordRoleDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.DiscordRoleDbModels.AsNoTracking().ToListAsync();
    }

    public async Task<DiscordRoleDbModel?> GetByIdAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    
        return await dbContext.DiscordRoleDbModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <param name="name"></param>
    /// <returns>Returns true, if Name already exists</returns>
    public async Task<bool> NameExistsAsync(string name)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    
        var check = await dbContext.DiscordRoleDbModels.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        
        return check is not null;
    }
    
    /// <param name="discordRoleId"></param>
    /// <returns>Returns true, if DiscordRoleId already exists</returns>
    public async Task<bool> DiscordRoleIdExistsAsync(ulong discordRoleId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    
        var check = await dbContext.DiscordRoleDbModels.AsNoTracking().FirstOrDefaultAsync(x => x.DiscordRoleId == discordRoleId);
        
        return check is not null;
    }
    
    public async Task<bool> AddAsync(DiscordRoleDbModel role, Ulid accessId)
    {
        if (await NameExistsAsync(role.Name)) return false;
        if (await DiscordRoleIdExistsAsync(role.DiscordRoleId)) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        await dbContext.DiscordRoleDbModels.AddAsync(role);
        await discordRoleLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            DiscordRoleLogTypeEnum.Add,
            accessId,
            $"Id: {role.Id}" +
            $"DiscordRoleId: {role.DiscordRoleId} \n" +
            $"Name: {role.Name} \n" +
            $"AutomaticRank: {role.AutomaticRank}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> RemoveAsync(Ulid roleId, Ulid accessId)
    {
        var role = await GetByIdAsync(roleId);
        if (role is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

         dbContext.DiscordRoleDbModels.Remove(role);
         await discordRoleLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            DiscordRoleLogTypeEnum.Remove,
            accessId,
            $"Id: {role.Id}" +
            $"DiscordRoleId: {role.DiscordRoleId} \n" +
            $"Name: {role.Name} \n" +
            $"AutomaticRank: {role.AutomaticRank}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateToPartial(
        DiscordRoleDbModel model,
        PartialDiscordRoleDbModel partialModel,
        Ulid accessId,
        bool updateName = true,
        bool updateDiscordRoleId = true,
        bool updateAutomaticRank = true
    )
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (updateName && model.Name != partialModel.Name)
        {
            var oldName = model.Name;
            model.Name = partialModel.Name;
            await discordRoleLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                DiscordRoleLogTypeEnum.ModifyName,
                accessId,
                $"Id: {model.Id} \n" +
                $"OldName: {oldName} \n" +
                $"NewName: {model.Name}"
            ));
        }
        if (updateDiscordRoleId && model.DiscordRoleId != partialModel.DiscordRoleId)
        {
            var oldDiscordRoleId = model.DiscordRoleId;
            model.DiscordRoleId = partialModel.DiscordRoleId;
            await discordRoleLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                DiscordRoleLogTypeEnum.ModifyDiscordRoleId,
                accessId,
                $"Id: {model.Id} \n" +
                $"OldDiscordRoleId: {oldDiscordRoleId} \n" +
                $"NewDiscordRoleId: {model.DiscordRoleId}"
            ));
        }
        if (updateAutomaticRank && model.AutomaticRank != partialModel.AutomaticRank)
        {
            var oldAutomaticRank = model.AutomaticRank;
            model.AutomaticRank = partialModel.AutomaticRank;
            await discordRoleLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                DiscordRoleLogTypeEnum.ModifyAutomaticRank,
                accessId,
                $"Id: {model.Id} \n" +
                $"OldAutomaticRank: {oldAutomaticRank} \n" +
                $"NewAutomaticRank: {model.AutomaticRank}"
            ));
        }
        
        dbContext.DiscordRoleDbModels.Update(model);
        
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
}