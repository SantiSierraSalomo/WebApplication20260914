using WebApplication20260914.Promotion.Dtos;

namespace WebApplication20260914.Promotion.Service;

public interface IPromotionParticipantService
{
    Task<IReadOnlyList<PromotionParticipantDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<PromotionParticipantDto?> GetByIdAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken);

    Task<PromotionParticipantDto> CreateAsync(CreatePromotionParticipantDto request, CancellationToken cancellationToken);

    Task<PromotionParticipantDto?> UpdateAsync(string promotionId, string itemId, string customerId, UpdatePromotionParticipantDto request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken);
}
