using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Promotion.Repository;

public interface IPromotionParticipantRepository
{
    Task<IReadOnlyList<PromotionParticipant>> GetAllAsync(CancellationToken cancellationToken);

    Task<PromotionParticipant?> GetByIdAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken);

    Task AddAsync(PromotionParticipant participant, CancellationToken cancellationToken);

    Task UpdateAsync(PromotionParticipant participant, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken);
}
