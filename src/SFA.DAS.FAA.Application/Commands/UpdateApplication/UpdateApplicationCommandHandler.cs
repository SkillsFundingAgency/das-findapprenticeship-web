using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication
{
    public class UpdateApplicationCommandHandler(IApiClient apiClient)
        : IRequestHandler<UpdateApplicationCommand, UpdateApplicationCommandResult>
    {
        public async Task<UpdateApplicationCommandResult> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new UpdateApplicationApiRequest(
                request.ApplicationId,
                request.CandidateId,
                new UpdateApplicationModel
                {
                    WorkHistorySectionStatus = request.WorkHistorySectionStatus
                });

            var response = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postRequest);

            return new UpdateApplicationCommandResult
            {
                Application = response
            };
        }
    }
}
