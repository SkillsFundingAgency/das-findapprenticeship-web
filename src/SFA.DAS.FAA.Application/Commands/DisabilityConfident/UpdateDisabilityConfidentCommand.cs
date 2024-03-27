using MediatR;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.DisabilityConfident
{
    public class UpdateDisabilityConfidentCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public bool ApplyUnderDisabilityConfidentScheme { get; set; }
    }

    public class UpdateDisabilityConfidentCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateDisabilityConfidentCommand>
    {
        public async Task Handle(UpdateDisabilityConfidentCommand request, CancellationToken cancellationToken)
        {
            var data = new PostDisabilityConfidentApiRequest.PostDisabilityConfidentApiRequestData
            {
                CandidateId = request.CandidateId,
                ApplyUnderDisabilityConfidentScheme = request.ApplyUnderDisabilityConfidentScheme
            };
            var apiRequest = new PostDisabilityConfidentApiRequest(request.ApplicationId, data);

            await apiClient.PostWithResponseCode(apiRequest);
        }
    }
}
