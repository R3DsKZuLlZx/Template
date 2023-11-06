using FluentAssertions;
using Moq;
using Template.Application.Customers;
using Template.Application.Customers.Common;
using Template.Application.Customers.GetCustomers;
using Xunit;

namespace Template.Unit.Tests.Customers.GetCustomers;

public class GetCustomersHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    
    public GetCustomersHandlerTests()
    {
        _mockCustomerRepository = new();
    }
    
    [Fact]
    public async Task Handler_Should_ReturnCustomers()
    {
        _mockCustomerRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Customer>());
        
        var handler = new GetCustomersHandler(_mockCustomerRepository.Object);

        var query = new GetCustomersQuery();

        var result = await handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
