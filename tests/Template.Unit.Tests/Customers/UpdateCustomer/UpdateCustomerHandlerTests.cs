using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Moq;
using Template.Application.Common.Interfaces;
using Template.Application.Customers;
using Template.Application.Customers.UpdateCustomer;
using Xunit;

namespace Template.Unit.Tests.Customers.UpdateCustomer;

public class UpdateCustomerHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    
    public UpdateCustomerHandlerTests()
    {
        _mockUnitOfWork = new();
        _mockCustomerRepository = new();
    }
    
    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenRequestIsValid()
    {
        var handler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<UpdateCustomerCommand>()
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.DateOfBirth, x => x.Person.DateOfBirth);

        var command = commandFaker.Generate();

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Fact]
    public async Task Handler_Should_ThrowValidationException_WhenIdIsInvalid()
    {
        var handler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<UpdateCustomerCommand>()
            .RuleFor(x => x.Id, Guid.Empty)
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.DateOfBirth, x => x.Person.DateOfBirth);

        var command = commandFaker.Generate();

        var result = () => handler.Handle(command, default);

        await result.Should().ThrowAsync<ArgumentException>();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handler_Should_ThrowValidationException_WhenUsernameIsInvalid()
    {
        var handler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<UpdateCustomerCommand>()
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => x.Person.UserName.ClampLength(31))
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.DateOfBirth, x => x.Person.DateOfBirth);

        var command = commandFaker.Generate();

        var result = () => handler.Handle(command, default);

        await result.Should().ThrowAsync<FluentValidation.ValidationException>();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handler_Should_ThrowValidationException_WhenFullNameIsInvalid()
    {
        var handler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<UpdateCustomerCommand>()
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.FullName, x => $"{x.Person.FullName}1")
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.DateOfBirth, x => x.Person.DateOfBirth);

        var command = commandFaker.Generate();

        var result = () => handler.Handle(command, default);

        await result.Should().ThrowAsync<FluentValidation.ValidationException>();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handler_Should_ThrowValidationException_WhenEmailIsInvalid()
    {
        var handler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<UpdateCustomerCommand>()
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Email, x => x.Person.Email.ClampLength(1, 1))
            .RuleFor(x => x.DateOfBirth, x => x.Person.DateOfBirth);

        var command = commandFaker.Generate();

        var result = () => handler.Handle(command, default);

        await result.Should().ThrowAsync<FluentValidation.ValidationException>();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handler_Should_ThrowValidationException_WhenDateOfBirthIsInTheFuture()
    {
        var handler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<UpdateCustomerCommand>()
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.DateOfBirth, DateTime.Now.AddDays(1));

        var command = commandFaker.Generate();

        var result = () => handler.Handle(command, default);

        await result.Should().ThrowAsync<FluentValidation.ValidationException>();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
