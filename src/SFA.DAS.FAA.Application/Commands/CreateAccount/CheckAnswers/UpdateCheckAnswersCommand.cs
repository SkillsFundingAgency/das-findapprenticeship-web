using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.CheckAnswers
{
    public class UpdateCheckAnswersCommand : IRequest
    {
        public Guid CandidateId { get; set; }
    }

    public class UpdateCheckAnswersCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateCheckAnswersCommand>
    {
        public async Task Handle(UpdateCheckAnswersCommand request, CancellationToken cancellationToken)
        {
            await apiClient.PostWithResponseCode<NullResponse>(new UpdateCheckAnswersApiRequest(request.CandidateId));
        }
    }
}
