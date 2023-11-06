using Moq;
using Template.Application.Common.Interfaces;
using Template.Application.Customers;
using Template.Application.Customers.DeleteCustomer;
using Xunit;

namespace Template.Unit.Tests.Customers.DeleteCustomer;

public class DeleteCustomerHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    
    public DeleteCustomerHandlerTests()
    {
        _mockUnitOfWork = new();
        _mockCustomerRepository = new();
    }
    
    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenCustomerIsDeleted()
    {
        _mockCustomerRepository
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        var handler = new DeleteCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var command = new DeleteCustomerCommand
        {
            Id = Guid.Empty,
        };

        await handler.Handle(command, default);

        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
