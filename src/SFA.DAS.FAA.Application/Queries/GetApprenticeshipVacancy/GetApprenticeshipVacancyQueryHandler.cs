using MediatR;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        private readonly IApiClient _apiClient;

        public GetApprenticeshipVacancyQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery query, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Get<GetApprenticeshipVacancyApiResponse>(new GetApprenticeshipVacancyApiRequest(query.VacancyReference));
            return new GetApprenticeshipVacancyQueryResult
            {
                Vacancy = response
            };
        }
    }
}
