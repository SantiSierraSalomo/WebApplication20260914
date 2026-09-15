using WebApplication20260914.Masterdata.Dtos;

namespace WebApplication20260914.Masterdata.Service;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<CustomerDto?> GetByIdAsync(string customerId, CancellationToken cancellationToken);

    Task<CustomerDto> CreateAsync(CreateCustomerDto request, CancellationToken cancellationToken);

    Task<CustomerDto?> UpdateAsync(string customerId, UpdateCustomerDto request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string customerId, CancellationToken cancellationToken);
}
