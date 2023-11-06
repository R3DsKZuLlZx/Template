using System.Net;
using FluentAssertions;
using MediatR;
using Moq;
using Template.Application.Customers.DeleteCustomer;
using Xunit;

namespace Template.Integration.Tests.Customers.DeleteCustomer;

public class DeleteCustomerTests
{
    [Fact]
    public async Task Endpoint_Should_Return204NoContent()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<DeleteCustomerCommand>(), It.IsAny<CancellationToken>()));

        var client = TestClient.CreateClient(mockMediatorSender);

        var id = Guid.NewGuid();
        
        var response = await client.DeleteAsync($"/customers/{id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
