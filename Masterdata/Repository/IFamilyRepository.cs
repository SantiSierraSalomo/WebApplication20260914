using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Masterdata.Repository;

public interface IFamilyRepository
{
    Task<IReadOnlyList<Family>> GetAllAsync(CancellationToken cancellationToken);

    Task<Family?> GetByIdAsync(string familyId, CancellationToken cancellationToken);

    Task AddAsync(Family family, CancellationToken cancellationToken);

    Task UpdateAsync(Family family, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string familyId, CancellationToken cancellationToken);
}
