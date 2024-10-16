using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Commands.User.PostAccountDeletion
{
    public record AccountDeletionCommandHandler(IApiClient ApiClient) : IRequestHandler<AccountDeletionCommand>
    {
        public async Task Handle(AccountDeletionCommand command, CancellationToken cancellationToken)
        {
            var apiRequest = new PostUserAccountDeletionApiRequest(command.CandidateId);

            await ApiClient.PostWithResponseCode(apiRequest);
        }
    }
}