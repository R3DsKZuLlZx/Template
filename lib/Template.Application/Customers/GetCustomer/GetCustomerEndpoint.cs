using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Template.Application.Customers.GetCustomer;

public class GetCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/customers/{id}",
            async ([FromServices] ISender sender, Guid id) =>
            {
                var query = new GetCustomerQuery
                {
                    Id = id,
                };
                
                var response = await sender.Send(query);
                if (!response.IsSuccess)
                {
                    return Results.BadRequest(response.Errors);
                }

                return Results.Ok(response.Value);
            });
    }
}
