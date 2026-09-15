using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data;
using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Promotion.Repository;

public sealed class PromotionParticipantRepository(AppDbContext dbContext) : IPromotionParticipantRepository
{
    public async Task<IReadOnlyList<PromotionParticipant>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.PromotionParticipants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PromotionParticipant?> GetByIdAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken)
    {
        return await dbContext.PromotionParticipants
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.PromotionId == promotionId && x.ItemId == itemId && x.CustomerId == customerId,
                cancellationToken);
    }

    public async Task AddAsync(PromotionParticipant participant, CancellationToken cancellationToken)
    {
        await dbContext.PromotionParticipants.AddAsync(participant, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<long> AddList(IReadOnlyList<PromotionParticipant> participants, CancellationToken cancellationToken)
    {
        await dbContext.PromotionParticipants.AddRangeAsync(participants, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PromotionParticipant participant, CancellationToken cancellationToken)
    {
        dbContext.PromotionParticipants.Update(participant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string promotionId, string itemId, string customerId, CancellationToken cancellationToken)
    {
        var participant = await dbContext.PromotionParticipants
            .FirstOrDefaultAsync(
                x => x.PromotionId == promotionId && x.ItemId == itemId && x.CustomerId == customerId,
                cancellationToken);

        if (participant is null)
        {
            return false;
        }

        dbContext.PromotionParticipants.Remove(participant);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
