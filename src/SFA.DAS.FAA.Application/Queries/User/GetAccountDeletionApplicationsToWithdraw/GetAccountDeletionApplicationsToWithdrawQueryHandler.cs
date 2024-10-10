using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw
{
    public record GetAccountDeletionApplicationsToWithdrawQueryHandler(IApiClient ApiClient) : IRequestHandler<GetAccountDeletionApplicationsToWithdrawQuery, GetAccountDeletionApplicationsToWithdrawQueryResult>
    {
        public async Task<GetAccountDeletionApplicationsToWithdrawQueryResult> Handle(GetAccountDeletionApplicationsToWithdrawQuery request, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetAccountDeletionApplicationsToWithdrawApiResponse>(
                new GetAccountDeletionApplicationsToWithdrawApiRequest(request.CandidateId));

            return response;
        }
    }
}