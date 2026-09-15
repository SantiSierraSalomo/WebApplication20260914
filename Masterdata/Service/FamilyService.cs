using WebApplication20260914.Data.Entities;
using WebApplication20260914.Masterdata.Dtos;
using WebApplication20260914.Masterdata.Repository;

namespace WebApplication20260914.Masterdata.Service;

public sealed class FamilyService(IFamilyRepository repository) : IFamilyService
{
    public async Task<IReadOnlyList<FamilyDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    public async Task<FamilyDto?> GetByIdAsync(string familyId, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(familyId, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<FamilyDto> CreateAsync(CreateFamilyDto request, CancellationToken cancellationToken)
    {
        var entity = new Family
        {
            FamilyId = request.FamilyId,
            Description = request.Description
        };

        await repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<FamilyDto?> UpdateAsync(string familyId, UpdateFamilyDto request, CancellationToken cancellationToken)
    {
        if (!string.Equals(familyId, request.FamilyId, StringComparison.Ordinal))
        {
            throw new ArgumentException("Route id and payload id must match.", nameof(familyId));
        }

        var existing = await repository.GetByIdAsync(familyId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        var updated = new Family
        {
            FamilyId = request.FamilyId,
            Description = request.Description
        };

        await repository.UpdateAsync(updated, cancellationToken);
        return MapToDto(updated);
    }

    public Task<bool> DeleteAsync(string familyId, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(familyId, cancellationToken);
    }

    private static FamilyDto MapToDto(Family entity)
    {
        return new FamilyDto(entity.FamilyId, entity.Description);
    }
}
