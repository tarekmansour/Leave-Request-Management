using Application.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Queries.GetLeaveRequestsByUser;

public sealed record GetLeaveRequestsByUserQuery(string? Status = null) : IRequest<Result<LeaveRequestsCollectionDto>>;
