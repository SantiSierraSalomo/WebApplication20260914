using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data;
using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Masterdata.Repository;

public sealed class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(string customerId, CancellationToken cancellationToken)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await dbContext.Customers.AddAsync(customer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        dbContext.Customers.Update(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string customerId, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);

        if (customer is null)
        {
            return false;
        }

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
