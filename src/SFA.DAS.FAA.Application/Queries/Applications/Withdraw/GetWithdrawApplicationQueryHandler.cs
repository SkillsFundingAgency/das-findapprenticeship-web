using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Applications.Withdraw;

public class GetWithdrawApplicationQueryHandler(IApiClient apiClient) : IRequestHandler<GetWithdrawApplicationQuery, GetWithdrawApplicationQueryResult>
{
    public async Task<GetWithdrawApplicationQueryResult> Handle(GetWithdrawApplicationQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetWithdrawApplicationApiResponse>(
            new GetWithdrawApplicationApiRequest(request.ApplicationId, request.CandidateId)
        );

        if (response == null) throw new ResourceNotFoundException();

        return new GetWithdrawApplicationQueryResult
        {
            ApplicationId = response.ApplicationId,
            ClosingDate = response.ClosingDate,
            ClosedDate = response.ClosedDate,
            EmployerName = response.EmployerName,
            SubmittedDate = response.SubmittedDate,
            AdvertTitle = response.AdvertTitle
        };
    }
}