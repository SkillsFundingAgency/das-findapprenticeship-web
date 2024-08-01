using MediatR;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Applications.Withdraw;

public class GetWithdrawApplicationQueryHandler(IApiClient apiClient) : IRequestHandler<GetWithdrawApplicationQuery, GetWithdrawApplicationQueryResult>
{
    public async Task<GetWithdrawApplicationQueryResult> Handle(GetWithdrawApplicationQuery request, CancellationToken cancellationToken)
    {
        var actual =
            await apiClient.Get<GetWithdrawApplicationApiResponse>(
                new GetWithdrawApplicationApiRequest(request.ApplicationId, request.CandidateId));

        if (actual == null) throw new ResourceNotFoundException();

        return new GetWithdrawApplicationQueryResult
        {
            ApplicationId = actual.ApplicationId,
            ClosingDate = actual.ClosingDate,
            EmployerName = actual.EmployerName,
            SubmittedDate = actual.SubmittedDate,
            AdvertTitle = actual.AdvertTitle
        };
    }
}