﻿using Application.Commands.CreateLeaveRequest;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands.CreateLeaveRequest;
public partial class CreateLeaveRequestCommandTests : DatabaseFixture
{
    private readonly ILogger<CreateLeaveRequestCommandHandler> _logger;
    private readonly CreateLeaveRequestCommandValidator _validator;
    private readonly ILeaveRequestRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateLeaveRequestCommandHandler _sut;

    public CreateLeaveRequestCommandTests()
    {
        _logger = Substitute.For<ILogger<CreateLeaveRequestCommandHandler>>();
        _validator = new CreateLeaveRequestCommandValidator();
        _clientRepository = new LeaveRequestRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
        _sut = new CreateLeaveRequestCommandHandler(_logger, _validator, _clientRepository, _unitOfWork);
    }
}