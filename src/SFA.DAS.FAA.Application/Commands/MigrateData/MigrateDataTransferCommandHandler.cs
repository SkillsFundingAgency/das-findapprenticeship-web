using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Commands.MigrateData
{
    public record MigrateDataTransferCommandHandler(IApiClient ApiClient)
        : IRequestHandler<MigrateDataTransferCommand, Unit>
    {
        public async Task<Unit> Handle(MigrateDataTransferCommand request, CancellationToken cancellationToken)
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