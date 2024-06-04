using MediatR;
using SFA.DAS.FAA.Domain.Applications.MigrateData;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.Applications.LegacyApplications
{
    public record MigrateLegacyApplicationsCommandHandler(IApiClient ApiClient)
        : IRequestHandler<MigrateLegacyApplicationsCommand, Unit>
    {
        public async Task<Unit> Handle(MigrateLegacyApplicationsCommand request, CancellationToken cancellationToken)
        {
            await ApiClient.PostWithResponseCode(
                new PostMigrateDataTransferApiRequest(request.CandidateId, new PostMigrateDataTransferApiRequest.PostMigrateDataTransferApiRequestData
                {
                    EmailAddress = request.EmailAddress
                }));

            return new Unit();
        }
    }
}