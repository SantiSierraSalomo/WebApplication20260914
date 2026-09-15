using WebApplication20260914.Data.Entities;
using WebApplication20260914.Promotion.Dtos;
using WebApplication20260914.Promotion.Repository;

namespace WebApplication20260914.Promotion.Service;

public sealed class PromotionParticipantService(IPromotionParticipantRepository repository) : IPromotionParticipantService
{
    public async Task<IReadOnlyList<PromotionParticipantDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    public async Task<PromotionParticipantDto?> GetByIdAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(promotionId, itemId, customerId, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<PromotionParticipantDto> CreateAsync(CreatePromotionParticipantDto request, CancellationToken cancellationToken)
    {
        var entity = new PromotionParticipant
        {
            PromotionId = request.PromotionId,
            ItemId = request.ItemId,
            CustomerId = request.CustomerId,
            BaseFrcQty = request.BaseFrcQty,
            Uplift = request.Uplift
        };

        await repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<PromotionParticipantDto?> UpdateAsync(string promotionId, string itemId, string customerId, UpdatePromotionParticipantDto request, CancellationToken cancellationToken)
    {
        if (!string.Equals(promotionId, request.PromotionId, StringComparison.Ordinal) ||
            !string.Equals(itemId, request.ItemId, StringComparison.Ordinal) ||
            !string.Equals(customerId, request.CustomerId, StringComparison.Ordinal))
        {
            throw new ArgumentException("Route keys and payload keys must match.");
        }

        var existing = await repository.GetByIdAsync(promotionId, itemId, customerId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.BaseFrcQty = request.BaseFrcQty;
        existing.Uplift = request.Uplift;

        await repository.UpdateAsync(existing, cancellationToken);
        return MapToDto(existing);
    }

    public Task<bool> DeleteAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(promotionId, itemId, customerId, cancellationToken);
    }

    private static PromotionParticipantDto MapToDto(PromotionParticipant entity)
    {
        return new PromotionParticipantDto(
            entity.PromotionId,
            entity.ItemId,
            entity.CustomerId,
            entity.BaseFrcQty,
            entity.Uplift);
    }
}
