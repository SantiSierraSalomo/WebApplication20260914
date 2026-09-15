using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Masterdata.Repository;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken);

    Task<Customer?> GetByIdAsync(string customerId, CancellationToken cancellationToken);

    Task AddAsync(Customer customer, CancellationToken cancellationToken);

    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string customerId, CancellationToken cancellationToken);
}
