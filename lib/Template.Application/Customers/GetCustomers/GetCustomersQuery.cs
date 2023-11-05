using FluentResults;
using Template.Application.Common.Cqrs;
using Template.Application.Customers.Common;

namespace Template.Application.Customers.GetCustomers;

public class GetCustomersQuery : Query<Result<List<Customer>>>
{
}
