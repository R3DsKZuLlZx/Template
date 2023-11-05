using Template.Application.Customers.Common;

namespace Template.Application.Customers;

public interface ICustomerRepository
{
    Task CreateAsync(Customer customer, CancellationToken cancellationToken);

    Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);

    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
