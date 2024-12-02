﻿using Application.Dtos;
using Domain.ValueObjects.Identifiers;
using MediatR;
using SharedKernel;

namespace Application.Commands.CreateLeaveRequest;
public record CreateLeaveRequestCommand(
    UserId SubmittedBy,
    LeaveTypeId LeaveTypeId,
    DateTime StartDate,
    DateTime EndDate,
    string? Comment = null) : IRequest<Result<CreatedLeaveRequestDto>>;
