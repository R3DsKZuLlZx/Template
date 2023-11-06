using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Template.Application.Customers.DeleteCustomer;

public class DeleteCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/customers/{id}", 
            async ([FromServices] ISender sender, Guid id) =>
            {
                var command = new DeleteCustomerCommand
                {
                    Id = id,
                };
                
                await sender.Send(command);
                return Results.NoContent();
            });
    }
}
