using MediatR;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.CandidateStatus
{
    public class UpdateCandidateStatusCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateCandidateStatusCommand, UpdateCandidateStatusCommandResult>
    {
        public async Task<UpdateCandidateStatusCommandResult> Handle(UpdateCandidateStatusCommand request, CancellationToken cancellationToken)
        {
            var candidateResponse = await apiClient.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(request.GovIdentifier, new PutCandidateApiRequestData { Email = request.CandidateEmail }));

            if (candidateResponse.Status == UserStatus.InProgress)
            {
                await apiClient.PostWithResponseCode(new UpdateCandidateStatusApiRequest(request.GovIdentifier, new UpdateCandidateStatusApiRequest.UpdateCandidateStatusApiRequestData
                {
                    Email = request.CandidateEmail,
                    Status = UserStatus.Completed
                }));
            }

            return candidateResponse;
        }
    }
}