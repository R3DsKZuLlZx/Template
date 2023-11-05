using Template.Application.Common.Cqrs;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Template.Architecture.Tests;

public class CommandTests
{
    [Fact]
    public void Commands_Should_BeNamedCommand()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(Command))
            .Or()
            .Inherit(typeof(Command<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Commands_Should_InheritCommand()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .DoNotHaveName("Command")
            .Should()
            .Inherit(typeof(Command))
            .Or()
            .Inherit(typeof(Command<>))
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}
