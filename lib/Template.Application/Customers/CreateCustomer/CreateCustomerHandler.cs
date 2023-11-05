using FluentResults;
using MediatR;
using Template.Application.Common.Interfaces;
using Template.Application.Customers.Common;

namespace Template.Application.Customers.CreateCustomer;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Result<Customer>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = CustomerId.From(Guid.NewGuid()),
            Email = EmailAddress.From(request.Email),
            Username = Username.From(request.Username),
            FullName = FullName.From(request.FullName),
            DateOfBirth = DateOfBirth.From(DateOnly.FromDateTime(request.DateOfBirth)),
        };

        var existingUser = await _customerRepository.GetAsync(customer.Id.Value, cancellationToken);
        if (existingUser is not null)
        {
            return Result.Fail<Customer>($"A user with id {customer.Id} already exists");
        }
        
        await _customerRepository.CreateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(customer);
    }
}
