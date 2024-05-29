using MediatR;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.Applications.Withdraw;

public class WithdrawApplicationCommandHandler(IApiClient apiClient) : IRequestHandler<WithdrawApplicationCommand, Unit>
{
    public async Task<Unit> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PostWithResponseCode(new PostWithdrawApplicationApiRequest(request.ApplicationId, request.CandidateId));
        
        return new Unit();
    }
}