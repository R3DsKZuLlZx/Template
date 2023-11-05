using Template.Application.Common.Cqrs;

namespace Template.Application.Customers.DeleteCustomer;

public class DeleteCustomerCommand : Command
{
    public Guid Id { get; set; }
}
