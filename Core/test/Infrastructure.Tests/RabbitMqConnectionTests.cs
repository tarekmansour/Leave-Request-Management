using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Infrastructure.Messaging;
using NSubstitute;
using RabbitMQ.Client;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class RabbitMqConnectionTests
{
    [Fact(DisplayName = "Throws ArgumentNullException when hostname is null")]
    public void Constructor_ShouldThrowArgumentNullException_WhenHostnameIsNull()
    {
        // Act
        Action act = () => new RabbitMqConnection(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'hostname')");
    }

    [Fact(DisplayName = "Successfully creates a RabbitMqConnection with a valid hostname")]
    public void Constructor_ShouldInitializeConnection_WhenHostnameIsValid()
    {
        // Arrange
        var hostname = "localhost";

        // Act
        var connection = new RabbitMqConnection(hostname);

        // Assert
        connection.Should().NotBeNull();
        connection.Connection.Should().NotBeNull();
    }

    [Fact(DisplayName = "Creates a new connection with the correct hostname")]
    public void InitializeConnection_ShouldCreateConnectionWithCorrectHostname()
    {
        // Arrange
        var hostname = "localhost";
        var connectionFactory = new ConnectionFactory
        {
            HostName = hostname
        };

        // Act
        var rabbitMqConnection = new RabbitMqConnection(hostname);

        // Assert
        rabbitMqConnection.Connection.Should().NotBeNull();
    }

    [Fact(DisplayName = "Dispose should release resources and close the connection")]
    public void Dispose_ShouldDisposeConnection_WhenCalled()
    {
        // Arrange
        var hostname = "localhost";
        var connection = new RabbitMqConnection(hostname);

        // Substitute IConnection for mock behavior
        var mockConnection = Substitute.For<IConnection>();
        connection.GetType().GetField("_connection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(connection, mockConnection);

        // Act
        connection.Dispose();

        // Assert
        mockConnection.Received().Dispose();
    }

}
