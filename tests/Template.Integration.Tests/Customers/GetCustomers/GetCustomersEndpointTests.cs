using System.Net;
using FluentAssertions;
using FluentResults;
using MediatR;
using Moq;
using Template.Application.Customers.Common;
using Template.Application.Customers.GetCustomers;
using Xunit;

namespace Template.Integration.Tests.Customers.GetCustomers;

public class GetCustomersEndpointTests
{
    [Fact]
    public async Task Endpoint_Should_Return200Ok_WhenRequestIsValid()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<GetCustomersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new List<Customer>()));

        var client = TestClient.CreateClient(mockMediatorSender);

        var response = await client.GetAsync("/customers");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Endpoint_Should_Return400BadRequest_WhenRequestIsInvalid()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<GetCustomersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<List<Customer>>(""));

        var client = TestClient.CreateClient(mockMediatorSender);

        var response = await client.GetAsync("/customers");
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
