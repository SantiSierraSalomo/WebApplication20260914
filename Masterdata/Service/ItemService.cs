using WebApplication20260914.Data.Entities;
using WebApplication20260914.Masterdata.Dtos;
using WebApplication20260914.Masterdata.Repository;

namespace WebApplication20260914.Masterdata.Service;

public sealed class ItemService(IItemRepository repository) : IItemService
{
    public async Task<IReadOnlyList<ItemDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    public async Task<ItemDto?> GetByIdAsync(string itemId, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(itemId, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<ItemDto> CreateAsync(CreateItemDto request, CancellationToken cancellationToken)
    {
        var entity = new Item
        {
            ItemId = request.ItemId,
            Description = request.Description,
            FamilyId = request.FamilyId,
            UnitCost = request.UnitCost,
            UnitVol = request.UnitVol
        };

        await repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<ItemDto?> UpdateAsync(string itemId, UpdateItemDto request, CancellationToken cancellationToken)
    {
        if (!string.Equals(itemId, request.ItemId, StringComparison.Ordinal))
        {
            throw new ArgumentException("Route id and payload id must match.", nameof(itemId));
        }

        var existing = await repository.GetByIdAsync(itemId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        var updated = new Item
        {
            ItemId = request.ItemId,
            Description = request.Description,
            FamilyId = request.FamilyId,
            UnitCost = request.UnitCost,
            UnitVol = request.UnitVol
        };

        await repository.UpdateAsync(updated, cancellationToken);
        return MapToDto(updated);
    }

    public Task<bool> DeleteAsync(string itemId, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(itemId, cancellationToken);
    }

    private static ItemDto MapToDto(Item entity)
    {
        return new ItemDto(entity.ItemId, entity.Description, entity.FamilyId, entity.UnitCost, entity.UnitVol);
    }
}
