using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedVacancies;

namespace SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy
{
    public record SaveVacancyCommandHandler : IRequestHandler<SaveVacancyCommand, SaveVacancyCommandResult>
    {
        private readonly IApiClient _apiClient;

        public SaveVacancyCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<SaveVacancyCommandResult> Handle(SaveVacancyCommand request, CancellationToken cancellationToken)
        {
            var data = new PostSaveVacancyApiRequestData
            {
                VacancyId = request.VacancyId
            };

            var apiRequest = new PostSaveVacancyApiRequest(request.CandidateId, data);

            var apiResponse = await _apiClient.PostWithResponseCode<PostSaveVacancyApiResponse>(apiRequest);

            if (apiResponse != null)
                return new SaveVacancyCommandResult
                {
                    Id = apiResponse.Id
                };

            return new SaveVacancyCommandResult();
        }
    }
}
