using WebApplication20260914.Masterdata.Dtos;

namespace WebApplication20260914.Masterdata.Service;

public interface IItemService
{
    Task<IReadOnlyList<ItemDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<ItemDto?> GetByIdAsync(string itemId, CancellationToken cancellationToken);

    Task<ItemDto> CreateAsync(CreateItemDto request, CancellationToken cancellationToken);

    Task<ItemDto?> UpdateAsync(string itemId, UpdateItemDto request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string itemId, CancellationToken cancellationToken);
}
