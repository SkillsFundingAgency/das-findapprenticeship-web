using MediatR;
using SFA.DAS.FAA.Domain.Apply.SubmitPreviewApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.SubmitApplication;

public class SubmitApplicationCommandHandler(IApiClient apiClient) : IRequestHandler<SubmitApplicationCommand, Unit>
{
    public async Task<Unit> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        var apiRequest = new SubmitPreviewApplicationRequest(request.CandidateId, request.ApplicationId);
        await apiClient.PostWithResponseCode(apiRequest);
        return new Unit();
    }
}