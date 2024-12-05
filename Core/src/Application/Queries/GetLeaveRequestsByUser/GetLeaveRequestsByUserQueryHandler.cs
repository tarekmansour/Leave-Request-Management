using Application.Abstractions;
using Application.Dtos;
using Domain.Repositories;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Queries.GetLeaveRequestsByUser;
public class GetLeaveRequestsByUserQueryHandler : IRequestHandler<GetLeaveRequestsByUserQuery, Result<LeaveRequestsCollectionDto>>
{
    private readonly ILogger _logger;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUserContext _userContext;

    public GetLeaveRequestsByUserQueryHandler(
        ILogger<GetLeaveRequestsByUserQueryHandler> logger,
        ILeaveRequestRepository leaveRequestRepository,
        IUserContext userContext)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    public async Task<Result<LeaveRequestsCollectionDto>> Handle(GetLeaveRequestsByUserQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(query));

        var leaveRequests = await _leaveRequestRepository.GetAllByUserAsync(
            userId: new UserId(_userContext.UserId),
            status: LeaveRequestStatus.FromNullableString(query.Status),
            cancellationToken);

        _logger.LogInformation("The list of leave requests for user '{UserId}' was retrieved with count of '{Count}' .",
            _userContext.UserId,
            leaveRequests.Count);

        return Result<LeaveRequestsCollectionDto>.Success(leaveRequests.MapToLeaveRequestsCollectionDto());
    }
}
