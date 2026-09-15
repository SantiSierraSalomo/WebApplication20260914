using WebApplication20260914.Promotion.Dtos;
using WebApplication20260914.Promotion.Repository;
using PromotionEntity = WebApplication20260914.Data.Entities.Promotion;

namespace WebApplication20260914.Promotion.Service;

public sealed class PromotionService(IPromotionRepository repository) : IPromotionService
{
    public async Task<IReadOnlyList<PromotionDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    public async Task<PromotionDto?> GetByIdAsync(string promotionId, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(promotionId, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<PromotionDto> CreateAsync(CreatePromotionDto request, CancellationToken cancellationToken)
    {
        var entity = new PromotionEntity
        {
            PromotionId = request.PromotionId,
            Description = request.Description,
            SellInStart = request.SellInStart,
            SellInEnd = request.SellInEnd,
            SellOutStart = request.SellOutStart,
            SellOutEnd = request.SellOutEnd
        };

        await repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<PromotionDto?> UpdateAsync(string promotionId, UpdatePromotionDto request, CancellationToken cancellationToken)
    {
        if (!string.Equals(promotionId, request.PromotionId, StringComparison.Ordinal))
        {
            throw new ArgumentException("Route id and payload id must match.", nameof(promotionId));
        }

        var existing = await repository.GetByIdAsync(promotionId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        var updated = new PromotionEntity
        {
            PromotionId = request.PromotionId,
            Description = request.Description,
            SellInStart = request.SellInStart,
            SellInEnd = request.SellInEnd,
            SellOutStart = request.SellOutStart,
            SellOutEnd = request.SellOutEnd
        };

        await repository.UpdateAsync(updated, cancellationToken);
        return MapToDto(updated);
    }

    public Task<bool> DeleteAsync(string promotionId, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(promotionId, cancellationToken);
    }

    private static PromotionDto MapToDto(PromotionEntity entity)
    {
        return new PromotionDto(
            entity.PromotionId,
            entity.Description,
            entity.SellInStart,
            entity.SellInEnd,
            entity.SellOutStart,
            entity.SellOutEnd);
    }
}
