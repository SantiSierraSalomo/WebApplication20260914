using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Promotion.Repository;

public interface IPromotionRepository
{
    Task<IReadOnlyList<Data.Entities.Promotion>> GetAllAsync(CancellationToken cancellationToken);

    Task<Data.Entities.Promotion?> GetByIdAsync(string promotionId, CancellationToken cancellationToken);

    Task AddAsync(Data.Entities.Promotion promotion, CancellationToken cancellationToken);

    Task UpdateAsync(Data.Entities.Promotion promotion, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string promotionId, CancellationToken cancellationToken);
}
