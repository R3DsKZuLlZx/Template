using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Template.Application.Customers.GetCustomers;

public class GetCustomersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/customers",
            async ([FromServices] ISender sender) =>
            {
                var query = new GetCustomersQuery();
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Errors);
                }
                
                return Results.Ok(result.Value);
            });
    }
}
