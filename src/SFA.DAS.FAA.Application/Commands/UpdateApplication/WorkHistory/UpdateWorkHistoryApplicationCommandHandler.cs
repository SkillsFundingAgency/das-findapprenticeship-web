using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory
{
    public class UpdateWorkHistoryApplicationCommandHandler(IApiClient apiClient)
        : IRequestHandler<UpdateWorkHistoryApplicationCommand, UpdateWorkHistoryApplicationCommandResult>
    {
        public async Task<UpdateWorkHistoryApplicationCommandResult> Handle(UpdateWorkHistoryApplicationCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new UpdateWorkHistoryApplicationApiRequest(
                            request.ApplicationId,
                            request.CandidateId,
                            new UpdateWorkHistoryApplicationModel
                            {
                                WorkHistorySectionStatus = request.WorkHistorySectionStatus
                            });

            var response = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postRequest);

            return new UpdateWorkHistoryApplicationCommandResult
            {
                Application = response
            };
        }
    }
}
