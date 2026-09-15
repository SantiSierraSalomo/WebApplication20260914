using WebApplication20260914.Data.Entities;
using WebApplication20260914.Masterdata.Dtos;
using WebApplication20260914.Masterdata.Repository;

namespace WebApplication20260914.Masterdata.Service;

public sealed class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(string customerId, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(customerId, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto request, CancellationToken cancellationToken)
    {
        var entity = new Customer
        {
            CustomerId = request.CustomerId,
            Description = request.Description,
            Country = request.Country,
            City = request.City,
            Address = request.Address,
            Longitude = request.Longitude,
            Latitude = request.Latitude
        };

        await repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<CustomerDto?> UpdateAsync(string customerId, UpdateCustomerDto request, CancellationToken cancellationToken)
    {
        if (!string.Equals(customerId, request.CustomerId, StringComparison.Ordinal))
        {
            throw new ArgumentException("Route id and payload id must match.", nameof(customerId));
        }

        var existing = await repository.GetByIdAsync(customerId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        var updated = new Customer
        {
            CustomerId = request.CustomerId,
            Description = request.Description,
            Country = request.Country,
            City = request.City,
            Address = request.Address,
            Longitude = request.Longitude,
            Latitude = request.Latitude
        };

        await repository.UpdateAsync(updated, cancellationToken);
        return MapToDto(updated);
    }

    public Task<bool> DeleteAsync(string customerId, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(customerId, cancellationToken);
    }

    private static CustomerDto MapToDto(Customer entity)
    {
        return new CustomerDto(
            entity.CustomerId,
            entity.Description,
            entity.Country,
            entity.City,
            entity.Address,
            entity.Longitude,
            entity.Latitude);
    }
}
