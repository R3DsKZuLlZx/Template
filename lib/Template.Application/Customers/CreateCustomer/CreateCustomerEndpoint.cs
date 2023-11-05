using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Template.Application.Customers.CreateCustomer;

public class CreateCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/customers", 
            async ([FromServices] ISender sender, [FromBody] CreateCustomerCommand command) =>
            {
                var response = await sender.Send(command);
                if (!response.IsSuccess)
                {
                    return Results.BadRequest(response.Errors);
                }

                return Results.Ok(response.Value);
            });
    }
}
