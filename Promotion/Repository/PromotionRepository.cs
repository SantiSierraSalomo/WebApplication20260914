using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data;
using PromotionEntity = WebApplication20260914.Data.Entities.Promotion;

namespace WebApplication20260914.Promotion.Repository;

public sealed class PromotionRepository(AppDbContext dbContext) : IPromotionRepository
{
    public async Task<IReadOnlyList<PromotionEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Promotions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PromotionEntity?> GetByIdAsync(string promotionId, CancellationToken cancellationToken)
    {
        return await dbContext.Promotions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PromotionId == promotionId, cancellationToken);
    }

    public async Task AddAsync(PromotionEntity promotion, CancellationToken cancellationToken)
    {
        await dbContext.Promotions.AddAsync(promotion, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PromotionEntity promotion, CancellationToken cancellationToken)
    {
        dbContext.Promotions.Update(promotion);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string promotionId, CancellationToken cancellationToken)
    {
        var promotion = await dbContext.Promotions
            .FirstOrDefaultAsync(x => x.PromotionId == promotionId, cancellationToken);

        if (promotion is null)
        {
            return false;
        }

        dbContext.Promotions.Remove(promotion);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
