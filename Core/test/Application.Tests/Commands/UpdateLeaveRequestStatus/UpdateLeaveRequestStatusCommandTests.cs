using System.Diagnostics.CodeAnalysis;
using Application.Commands.UpdateLeaveRequestStatus;
using Domain.Repositories;
using Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands.UpdateLeaveRequestStatus;

[ExcludeFromCodeCoverage]
public partial class UpdateLeaveRequestStatusCommandTests : DatabaseFixture
{
    private readonly ILogger<UpdateLeaveRequestStatusCommandHandler> _logger;
    private readonly UpdateLeaveRequestStatusCommandValidator _validator;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateLeaveRequestStatusCommandHandler _sut;

    public UpdateLeaveRequestStatusCommandTests()
    {
        _logger = Substitute.For<ILogger<UpdateLeaveRequestStatusCommandHandler>>();
        _leaveRequestRepository = Substitute.For<ILeaveRequestRepository>();
        _validator = new UpdateLeaveRequestStatusCommandValidator(_leaveRequestRepository);
        _unitOfWork = new UnitOfWork(_dbContext);
        _sut = new UpdateLeaveRequestStatusCommandHandler(_logger, _validator, _leaveRequestRepository, _unitOfWork);
    }
}
