using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Masterdata.Repository;

public interface IItemRepository
{
    Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken);

    Task<Item?> GetByIdAsync(string itemId, CancellationToken cancellationToken);

    Task AddAsync(Item item, CancellationToken cancellationToken);

    Task UpdateAsync(Item item, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string itemId, CancellationToken cancellationToken);
}
