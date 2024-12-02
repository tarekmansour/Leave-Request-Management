using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows.Input;
using Api;
using Domain.Entities;
using FluentAssertions;
using Infrastructure;
using NetArchTest.Rules;

namespace Architecture.Tests;

[ExcludeFromCodeCoverage]
public class LayersDependencyTests
{
    protected static readonly Assembly DomainAssembly = typeof(LeaveRequest).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;

    [Fact(DisplayName = "Domain layer should not have dependency on application layer")]
    public void Domain_Should_NotHaveDependencyOn_Application()
    {
        // Arrange & Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Domain layer should not have dependency on infrastructure layer")]
    public void Domain_Should_NotHaveDependencyOn_Infrastructure()
    {
        // Arrange & Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Domain layer should not have dependency on presentation layer")]
    public void Domain_Should_NotHaveDependencyOn_Presentation()
    {
        // Arrange & Act
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Application layer should not have dependency on presentation layer")]
    public void Application_Should_NotHaveDependencyOn_Presentation()
    {
        // Arrange & Act
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Infrastructure layer should not have dependency on presentation layer")]
    public void Infrastructure_ShouldNotHaveDependencyOn_Presentation()
    {
        // Arrange & Act
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}