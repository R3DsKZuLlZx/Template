using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Template.Application.Customers.UpdateCustomer;

public class UpdateCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/customers", 
            async ([FromServices] ISender sender, [FromBody] UpdateCustomerCommand command) =>
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
