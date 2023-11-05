using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Template.Architecture.Tests;

public class ArchitectureTests
{
    private const string InfrastructureNamespace = "Template.Infrastructure";
    private const string WebNamespace = "Template.Web";
    
    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Application.TemplateApplicationExtensions).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            WebNamespace,
        };
        
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}
