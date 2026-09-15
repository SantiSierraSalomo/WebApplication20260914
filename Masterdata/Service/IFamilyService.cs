using WebApplication20260914.Masterdata.Dtos;

namespace WebApplication20260914.Masterdata.Service;

public interface IFamilyService
{
    Task<IReadOnlyList<FamilyDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<FamilyDto?> GetByIdAsync(string familyId, CancellationToken cancellationToken);

    Task<FamilyDto> CreateAsync(CreateFamilyDto request, CancellationToken cancellationToken);

    Task<FamilyDto?> UpdateAsync(string familyId, UpdateFamilyDto request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string familyId, CancellationToken cancellationToken);
}
