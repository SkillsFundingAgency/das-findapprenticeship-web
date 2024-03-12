using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.DisabilityConfident;

public record UpdateDisabilityConfidenceApplicationCommandHandler(IApiClient ApiClient)
    : IRequestHandler<UpdateDisabilityConfidenceApplicationCommand, UpdateDisabilityConfidenceApplicationCommandResult>
{
    public async Task<UpdateDisabilityConfidenceApplicationCommandResult> Handle
        (UpdateDisabilityConfidenceApplicationCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new UpdateDisabilityConfidenceApplicationApiRequest(
            request.ApplicationId,
            request.CandidateId,
            new UpdateDisabilityConfidenceApplicationModel
            {
                DisabilityConfidenceModelSectionStatus = request.DisabilityConfidenceSectionStatus
            });

        var response = await ApiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postRequest);

        return new UpdateDisabilityConfidenceApplicationCommandResult
        {
            Application = response
        };
    }
}