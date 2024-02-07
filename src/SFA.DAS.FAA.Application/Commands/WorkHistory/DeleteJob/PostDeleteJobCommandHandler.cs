using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class PostDeleteJobCommandHandler : IRequestHandler<PostDeleteJobCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public PostDeleteJobCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(PostDeleteJobCommand command, CancellationToken cancellationToken)
        {
            var request = new PostDeleteJobApiRequest(command.ApplicationId, command.JobId, new PostDeleteJobApiRequest.PostDeleteJobApiRequestData
            {
                CandidateId = command.CandidateId,
            });

            await _apiClient.PostWithResponseCode(request);
            return Unit.Value;
        }
    }

}
