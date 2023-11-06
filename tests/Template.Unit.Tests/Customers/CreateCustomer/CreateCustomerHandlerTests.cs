using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Moq;
using Template.Application.Common.Interfaces;
using Template.Application.Customers;
using Template.Application.Customers.Common;
using Template.Application.Customers.CreateCustomer;
using Xunit;

namespace Template.Unit.Tests.Customers.CreateCustomer;

public class CreateCustomerHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    
    public CreateCustomerHandlerTests()
    {
        _mockUnitOfWork = new();
        _mockCustomerRepository = new();
    }
    
    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenIdIsUnique()
    {
        var handler = new CreateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<CreateCustomerCommand>()
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
    public async Task Handler_Should_ReturnFailure_WhenIdIsNotUnique()
    {
        _mockCustomerRepository.Setup(
                x => x.GetAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer());
        
        var handler = new CreateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<CreateCustomerCommand>()
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Email, x => x.Person.Email)
            .RuleFor(x => x.DateOfBirth, x => x.Person.DateOfBirth);

        var command = commandFaker.Generate();

        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeNull();
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handler_Should_ThrowValidationException_WhenUsernameIsInvalid()
    {
        var handler = new CreateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<CreateCustomerCommand>()
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
        var handler = new CreateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<CreateCustomerCommand>()
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
        var handler = new CreateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<CreateCustomerCommand>()
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
        var handler = new CreateCustomerHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);

        var commandFaker = new Faker<CreateCustomerCommand>()
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
