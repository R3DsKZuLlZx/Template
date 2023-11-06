using System.Net;
using FluentAssertions;
using FluentResults;
using MediatR;
using Moq;
using Template.Application.Customers.Common;
using Template.Application.Customers.GetCustomer;
using Xunit;

namespace Template.Integration.Tests.Customers.GetCustomer;

public class GetCustomerEndpointTests
{
    [Fact]
    public async Task Endpoint_Should_Return200Ok_WhenRequestIsValid()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new Customer()));

        var client = TestClient.CreateClient(mockMediatorSender);

        var id = Guid.NewGuid();
        
        var response = await client.GetAsync($"/customers/{id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Endpoint_Should_Return400BadRequest_WhenRequestIsInvalid()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<Customer>(""));

        var client = TestClient.CreateClient(mockMediatorSender);

        var id = Guid.NewGuid();
        
        var response = await client.GetAsync($"/customers/{id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
