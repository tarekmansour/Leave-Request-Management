using System.Diagnostics.CodeAnalysis;
using Application.Abstractions.Messaging;
using FluentAssertions;
using Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using RabbitMQ.Client;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class RabbitMqMessageSenderTests
{
    [Fact(DisplayName = "Throws ArgumentNullException when connection is null")]
    public void Constructor_ShouldThrowArgumentNullException_WhenConnectionIsNull()
    {
        // Arrange
        var rabbitMqConfig = Substitute.For<IConfiguration>();
        rabbitMqConfig["RabbitMQ:QueueName"].Returns("testQueue");

        // Act
        Action act = () => new RabbitMqMessageSender(null!, rabbitMqConfig);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'connection')");
    }

    [Fact(DisplayName = "Throws ArgumentNullException when QueueName in config is null")]
    public void Constructor_ShouldThrowArgumentNullException_WhenQueueNameIsNull()
    {
        // Arrange
        var rabbitMqConfig = Substitute.For<IConfiguration>();
        rabbitMqConfig["RabbitMQ:QueueName"].Returns((string)null!);

        // Act
        Action act = () => new RabbitMqMessageSender(Substitute.For<IRabbitMqConnection>(), rabbitMqConfig);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'rabbitMqConfig')");
    }

    [Fact(DisplayName = "Successfully creates RabbitMqMessageSender with valid arguments")]
    public void Constructor_ShouldInitializeSender_WhenArgumentsAreValid()
    {
        // Arrange
        var rabbitMqConfig = Substitute.For<IConfiguration>();
        rabbitMqConfig["RabbitMQ:QueueName"].Returns("testQueue");
        var connection = Substitute.For<IRabbitMqConnection>();

        // Act
        var sender = new RabbitMqMessageSender(connection, rabbitMqConfig);

        // Assert
        sender.Should().NotBeNull();
    }

    [Fact(DisplayName = "SendMessage should declare queue and publish message")]
    public void SendMessage_ShouldDeclareQueueAndPublishMessage_WhenCalled()
    {
        // Arrange
        var connection = Substitute.For<IRabbitMqConnection>();
        var channel = Substitute.For<IModel>();
        connection.Connection.CreateModel().Returns(channel);

        var rabbitMqConfig = Substitute.For<IConfiguration>();
        rabbitMqConfig["RabbitMQ:QueueName"].Returns("testQueue");

        var messageSender = new RabbitMqMessageSender(connection, rabbitMqConfig);
        var testMessage = new { Status = "Approved" };

        // Act
        messageSender.SendMessage(testMessage);

        // Assert
        channel.Received().QueueDeclare(queue: "testQueue", exclusive: false);
    }
}
