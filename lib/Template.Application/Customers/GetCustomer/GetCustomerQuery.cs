using FluentResults;
using Template.Application.Common.Cqrs;
using Template.Application.Customers.Common;

namespace Template.Application.Customers.GetCustomer;

public class GetCustomerQuery : Query<Result<Customer>>
{
    public Guid Id { get; set; }
}
