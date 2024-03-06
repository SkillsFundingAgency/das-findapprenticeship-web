using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
public class UpdateInterviewAdjustmentsCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateInterviewAdjustmentsCommand, UpdateInterviewAdjustmentsCommandResult>
{
    public async Task<UpdateInterviewAdjustmentsCommandResult> Handle(UpdateInterviewAdjustmentsCommand request, CancellationToken cancellationToken)
    {
        var postUpdateApplicationRequest = new PostInterviewAdjustmentsApiRequest(
                    request.ApplicationId,
                    new PostInterviewAdjustmentsModel
                    {
                        CandidateId = request.CandidateId,
                        InterviewAdjustmentsDescription = request.InterviewAdjustmentsDescription,
                        InterviewAdjustmentsSectionStatus = request.InterviewAdjustmentsSectionStatus
                    });

        var updateApplicationResponse = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postUpdateApplicationRequest);

        return new UpdateInterviewAdjustmentsCommandResult
        {
            Application = updateApplicationResponse
        };
    }
}
