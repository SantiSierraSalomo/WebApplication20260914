using WebApplication20260914.Promotion.Dtos;

namespace WebApplication20260914.Promotion.Service;

public interface IPromotionService
{
    Task<IReadOnlyList<PromotionDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<PromotionDto?> GetByIdAsync(string promotionId, CancellationToken cancellationToken);

    Task<PromotionDto> CreateAsync(CreatePromotionDto request, CancellationToken cancellationToken);

    Task<PromotionDto?> UpdateAsync(string promotionId, UpdatePromotionDto request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string promotionId, CancellationToken cancellationToken);
}
