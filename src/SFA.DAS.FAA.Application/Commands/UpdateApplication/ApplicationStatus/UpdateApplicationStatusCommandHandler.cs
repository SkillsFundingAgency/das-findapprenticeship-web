using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.ApplicationStatus
{
    public record UpdateApplicationStatusCommandHandler(IApiClient ApiClient) : IRequestHandler<UpdateApplicationStatusCommand, UpdateApplicationStatusCommandResult>
    {
        public async Task<UpdateApplicationStatusCommandResult> Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new UpdateApplicationStatusApiRequest(
                request.ApplicationId,
                request.CandidateId,
                new UpdateApplicationStatusModel
                {
                    Status = request.Status
                });

            var response = await ApiClient.Post<Domain.Apply.UpdateApplication.Application>(postRequest);

            return new UpdateApplicationStatusCommandResult
            {
                Application = response
            };
        }
    }
}
