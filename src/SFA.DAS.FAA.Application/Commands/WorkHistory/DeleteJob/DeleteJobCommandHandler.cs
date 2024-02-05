using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public DeleteJobCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(DeleteJobCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.JobId);

            await _apiClient.Delete(request);
            return Unit.Value;
        }
    }

}
