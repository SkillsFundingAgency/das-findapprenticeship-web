using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpsertApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpsertApplication
{
    public class UpsertApplicationCommandHandler : IRequestHandler<UpsertApplicationCommand, UpsertApplicationCommandResult>
    {
        private readonly IApiClient _apiClient;

        public UpsertApplicationCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<UpsertApplicationCommandResult> Handle(UpsertApplicationCommand request, CancellationToken cancellationToken)
        {
            var patchRequest = new PatchApplicationApiRequest(request.VacancyReference, request.ApplicationId,
                request.CandidateId, new UpdateApplicationModel
                {
                    WorkHistorySectionStatus = request.WorkHistorySectionStatus
                });

            var response = await _apiClient.PatchWithResponseCode<Domain.Apply.UpsertApplication.Application>(patchRequest);

            return new UpsertApplicationCommandResult
            {
                Application = response
            };
        }
    }
}
