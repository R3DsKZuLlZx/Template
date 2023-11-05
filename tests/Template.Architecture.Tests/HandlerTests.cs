using Template.Unit.Tests;
using FluentAssertions;
using MediatR;
using NetArchTest.Rules;
using Xunit;

namespace Template.Architecture.Tests;

public class HandlerTests
{
    [Fact]
    public void Handlers_Should_BeSealed()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Handlers_Should_BeNamedHandler()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Handlers_Should_ImplementIRequestHandler()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Handlers_Should_HaveUnitTests()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;
        var testAssembly = typeof(AssemblyReference).Assembly;
        
        // Act
        var handlers = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .GetTypes()
            .Select(x => x.Name);

        var handlerTests = Types
            .InAssembly(testAssembly)
            .That()
            .HaveNameEndingWith("HandlerTests")
            .GetTypes()
            .Select(x => x.Name);

        var handlersWithoutTests = handlers.Where(h => !handlerTests.Any(ht => ht.Contains(h)));

        // Assert
        handlersWithoutTests.Should().BeEmpty();
    }
}
