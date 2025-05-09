using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedVacancies;

namespace SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommandHandler : IRequestHandler<DeleteSavedVacancyCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public DeleteSavedVacancyCommandHandler(IApiClient apiClient) => _apiClient = apiClient;

        public async Task<Unit> Handle(DeleteSavedVacancyCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostDeleteSavedVacancyApiRequest(request.CandidateId, new PostDeleteSavedVacancyApiRequestData
            {
                VacancyId = request.VacancyId,
                DeleteAllByVacancyReference = request.DeleteAllByVacancyReference
            });
            await _apiClient.PostWithResponseCode(apiRequest);

            return Unit.Value;
        }
    }
}