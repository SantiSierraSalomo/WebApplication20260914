using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data;
using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Masterdata.Repository;

public sealed class FamilyRepository(AppDbContext dbContext) : IFamilyRepository
{
    public async Task<IReadOnlyList<Family>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Families
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Family?> GetByIdAsync(string familyId, CancellationToken cancellationToken)
    {
        return await dbContext.Families
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FamilyId == familyId, cancellationToken);
    }

    public async Task AddAsync(Family family, CancellationToken cancellationToken)
    {
        await dbContext.Families.AddAsync(family, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Family family, CancellationToken cancellationToken)
    {
        dbContext.Families.Update(family);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string familyId, CancellationToken cancellationToken)
    {
        var family = await dbContext.Families
            .FirstOrDefaultAsync(x => x.FamilyId == familyId, cancellationToken);

        if (family is null)
        {
            return false;
        }

        dbContext.Families.Remove(family);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
