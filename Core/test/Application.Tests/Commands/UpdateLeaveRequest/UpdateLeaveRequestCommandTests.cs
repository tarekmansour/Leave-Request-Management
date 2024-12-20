﻿using System.Diagnostics.CodeAnalysis;
using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Commands.UpdateLeaveRequest;
using Domain.Repositories;
using Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands.UpdateLeaveRequest;

[ExcludeFromCodeCoverage]
public partial class UpdateLeaveRequestCommandTests : DatabaseFixture
{
    private readonly ILogger<UpdateLeaveRequestCommandHandler> _logger;
    private readonly UpdateLeaveRequestCommandValidator _validator;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateLeaveRequestCommandHandler _sut;
    private readonly IUserContext _userContext;
    private readonly IMessageSender _messageSender;


    public UpdateLeaveRequestCommandTests()
    {
        _logger = Substitute.For<ILogger<UpdateLeaveRequestCommandHandler>>();
        _leaveRequestRepository = Substitute.For<ILeaveRequestRepository>();
        _validator = new UpdateLeaveRequestCommandValidator();
        _unitOfWork = new UnitOfWork(_dbContext);
        _userContext = Substitute.For<IUserContext>();
        _messageSender = Substitute.For<IMessageSender>();
        _sut = new UpdateLeaveRequestCommandHandler(
            _logger,
            _validator,
            _leaveRequestRepository,
            _unitOfWork,
            _userContext,
            _messageSender);
    }
}
