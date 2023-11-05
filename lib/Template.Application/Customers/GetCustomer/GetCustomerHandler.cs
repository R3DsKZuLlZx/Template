using FluentResults;
using MediatR;
using Template.Application.Customers.Common;

namespace Template.Application.Customers.GetCustomer;

public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, Result<Customer>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<Result<Customer>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            return Result.Fail<Customer>($"No user exists with id {request.Id}");
        }

        return Result.Ok(customer);
    }
}
