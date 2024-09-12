using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Ranks.DbModels;
using ChoiceVFileSystemBlazor.Database.Ranks.Proxies.Intefaces;
using ChoiceVSharedApiModels.Accounts;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Ranks.Proxies;

public class RankProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IRankProxy
{
    public async Task<List<RightToRankDbModel>> GetAllRightsAsync(RankEnum rank)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.RightToRankDbModels.AsNoTracking().Where(x => x.Rank == rank).ToListAsync();
    }

    // Return null if adding failed
    public async Task<RightToRankDbModel?> AddRightToRankAsync(RightToRankDbModel rightToRankDbModel)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.RightToRankDbModels.AddAsync(rightToRankDbModel);
        // TODO Logs hinzufügen
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : rightToRankDbModel;
    }

    // Return false if removing failed
    public async Task<bool> DeleteRightToRankAsync(RankEnum rank, RightEnum right)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var rightToRankDbModel = await dbContext.RightToRankDbModels.AsNoTracking().FirstOrDefaultAsync(x => x.Rank == rank && x.Right == right);
        if (rightToRankDbModel is null)
        {
            return false;
        }
        
        dbContext.RightToRankDbModels.Remove(rightToRankDbModel);
        // TODO Logs hinzufügen
        var changes = await dbContext.SaveChangesAsync();        
        
        return changes > 0;
    }

    public async Task<(int, int, int)> ChangeBulkAsync(Dictionary<(RankEnum, RightEnum),bool> values)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        List<RightToRankDbModel> addList = [];
        List<RightToRankDbModel> removeList = [];
        
        var addedCount = 0;
        var removedCount = 0;
        
        var rightToRankDbModels = await dbContext.RightToRankDbModels.AsNoTracking().ToListAsync();
        
        foreach (var entry in values)
        {
            var rank = entry.Key.Item1;
            var right = entry.Key.Item2;
            var isChecked = entry.Value;

            var rightToRankDbModel = rightToRankDbModels.FirstOrDefault(x => x.Rank == rank && x.Right == right);
            if (isChecked && rightToRankDbModel is null)
            {
                addList.Add(new() { Rank = rank, Right = right });
                addedCount++;
            }
            else if (!isChecked && rightToRankDbModel is not null)
            {
                removeList.Add(rightToRankDbModel);
                removedCount++;
            }
        }
        
        dbContext.RightToRankDbModels.AddRange(addList);
        dbContext.RightToRankDbModels.RemoveRange(removeList);
        // TODO Logs hinzufügen
        var changes = await dbContext.SaveChangesAsync();  

        return (changes, addedCount, removedCount);
    }
}