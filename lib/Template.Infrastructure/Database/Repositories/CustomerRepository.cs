using Microsoft.EntityFrameworkCore;
using Template.Application.Customers;
using Template.Application.Customers.Common;
using Template.Infrastructure.Database.Entities;

namespace Template.Infrastructure.Database.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        var entity = new CustomerEntity
        {
            Id = customer.Id.Value,
            Username = customer.Username.Value,
            FullName = customer.FullName.Value,
            Email = customer.Email.Value,
            DateOfBirth = customer.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue),
        };

        await _dbContext.Customers.AddAsync(entity, cancellationToken);
    }

    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }
        
        return new Customer
        {
            Id = CustomerId.From(entity.Id),
            Email = EmailAddress.From(entity.Email),
            Username = Username.From(entity.Username),
            FullName = FullName.From(entity.FullName),
            DateOfBirth = DateOfBirth.From(DateOnly.FromDateTime(entity.DateOfBirth)),
        };
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _dbContext.Customers.ToListAsync(cancellationToken);

        var customers = entities.Select(x =>
            new Customer
            {
                Id = CustomerId.From(x.Id),
                Email = EmailAddress.From(x.Email),
                Username = Username.From(x.Username),
                FullName = FullName.From(x.FullName),
                DateOfBirth = DateOfBirth.From(DateOnly.FromDateTime(x.DateOfBirth)),
            }).ToList();

        return customers;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id.Value, cancellationToken);
        if (entity is null)
        {
            return;
        }
        
        entity.Username = customer.Username.Value;
        entity.FullName = customer.FullName.Value;
        entity.Email = customer.Email.Value;
        entity.DateOfBirth = customer.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _dbContext.Remove(entity);
    }
}
