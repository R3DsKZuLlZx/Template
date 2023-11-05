using FluentResults;
using Template.Application.Common.Cqrs;
using Template.Application.Customers.Common;

namespace Template.Application.Customers.UpdateCustomer;

public class UpdateCustomerCommand : Command<Result<Customer>>
{
    public Guid Id { get; set; }
    
    public string Username { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }
}
