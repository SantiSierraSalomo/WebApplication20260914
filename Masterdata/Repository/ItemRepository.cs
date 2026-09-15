using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data;
using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Masterdata.Repository;

public sealed class ItemRepository(AppDbContext dbContext) : IItemRepository
{
    public async Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Items
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Item?> GetByIdAsync(string itemId, CancellationToken cancellationToken)
    {
        return await dbContext.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ItemId == itemId, cancellationToken);
    }

    public async Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        await dbContext.Items.AddAsync(item, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Item item, CancellationToken cancellationToken)
    {
        dbContext.Items.Update(item);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string itemId, CancellationToken cancellationToken)
    {
        var item = await dbContext.Items
            .FirstOrDefaultAsync(x => x.ItemId == itemId, cancellationToken);

        if (item is null)
        {
            return false;
        }

        dbContext.Items.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
