using MediatR;
using SFA.DAS.FAA.Domain.Applications.MigrateApplications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.Applications.LegacyApplications
{
    public record MigrateLegacyApplicationsCommandHandler(IApiClient ApiClient)
        : IRequestHandler<MigrateLegacyApplicationsCommand, Unit>
    {
        public async Task<Unit> Handle(MigrateLegacyApplicationsCommand request, CancellationToken cancellationToken)
        {
            await ApiClient.PostWithResponseCode(
                new PostMigrateLegacyApplicationsApiRequest(request.CandidateId, new PostMigrateLegacyApplicationsApiRequest.PostMigrateLegacyApplicationsRequestData
                {
                    EmailAddress = request.EmailAddress
                }));

            return new Unit();
        }
    }
}