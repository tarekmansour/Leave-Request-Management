using System.Diagnostics.CodeAnalysis;
using Application.Abstractions.Authentication;
using Application.Queries.GetLeaveRequestsByUser;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Queries;

[ExcludeFromCodeCoverage]
public class GetLeaveRequestsByUserTests : DatabaseFixture
{
    private readonly ILogger<GetLeaveRequestsByUserQueryHandler> _logger;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUserContext _userContext;
    private readonly GetLeaveRequestsByUserQueryHandler _sut;

    public GetLeaveRequestsByUserTests()
    {
        _logger = Substitute.For<ILogger<GetLeaveRequestsByUserQueryHandler>>();
        _leaveRequestRepository = new LeaveRequestRepository(_dbContext);
        _userContext = Substitute.For<IUserContext>();
        _sut = new GetLeaveRequestsByUserQueryHandler(_logger, _leaveRequestRepository, _userContext);
    }

    [Fact(DisplayName = "new GetLeaveRequestsByUserQueryHandler with null repository")]
    public void WithNullRepository_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new GetLeaveRequestsByUserQueryHandler(
            _logger,
            null!,
            _userContext);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "new GetLeaveRequestsByUserQueryHandler with null userContext")]
    public void WithNullUserContext_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new GetLeaveRequestsByUserQueryHandler(
            _logger,
            _leaveRequestRepository,
            null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Handle returns successful result")]
    public async Task Handle_Should_ReturnsSuccessfulResult()
    {
        //Arrange
        const int mockUserId = 1;
        _userContext.UserId.Returns(mockUserId);

        var query = new GetLeaveRequestsByUserQuery("Approved");

        //Act
        var result = await _sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
