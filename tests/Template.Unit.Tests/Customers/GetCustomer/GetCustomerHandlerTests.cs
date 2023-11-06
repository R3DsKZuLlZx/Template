using FluentAssertions;
using Moq;
using Template.Application.Customers;
using Template.Application.Customers.Common;
using Template.Application.Customers.GetCustomer;
using Xunit;

namespace Template.Unit.Tests.Customers.GetCustomer;

public class GetCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    
    public GetCustomerHandlerTests()
    {
        _mockCustomerRepository = new();
    }
    
    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenCustomerExists()
    {
        _mockCustomerRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer());
        
        var handler = new GetCustomerHandler(_mockCustomerRepository.Object);

        var query = new GetCustomerQuery
        {
            Id = Guid.Empty,
        };

        var result = await handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        _mockCustomerRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);
        
        var handler = new GetCustomerHandler(_mockCustomerRepository.Object);

        var query = new GetCustomerQuery
        {
            Id = Guid.Empty,
        };

        var result = await handler.Handle(query, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeNull();
    }
}
