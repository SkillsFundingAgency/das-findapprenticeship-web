using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateQualifications
{
    public class UpdateQualificationsCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public bool IsComplete { get; set; }
    }

    public class UpdateQualificationsCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateQualificationsCommand>
    {
        public async Task Handle(UpdateQualificationsCommand request, CancellationToken cancellationToken)
        {
            var data = new PostQualificationsApiRequest.PostQualificationsApiRequestData
            {
                CandidateId = request.CandidateId,
                IsComplete = request.IsComplete
            };
            var apiRequest = new PostQualificationsApiRequest(request.ApplicationId, data);

            await apiClient.PostWithResponseCode(apiRequest);
        }
    }
}
