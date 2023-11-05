using FluentResults;
using MediatR;
using Template.Application.Customers.Common;

namespace Template.Application.Customers.GetCustomers;

public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, Result<List<Customer>>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<Result<List<Customer>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);
        return Result.Ok(customers.ToList());
    }
}
