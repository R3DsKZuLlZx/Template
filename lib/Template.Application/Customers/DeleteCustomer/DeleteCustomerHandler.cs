using MediatR;
using Template.Application.Common.Interfaces;

namespace Template.Application.Customers.DeleteCustomer;

public sealed class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await _customerRepository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
