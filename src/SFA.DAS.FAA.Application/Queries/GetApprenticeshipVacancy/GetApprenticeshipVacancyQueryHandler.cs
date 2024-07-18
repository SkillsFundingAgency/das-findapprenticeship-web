using MediatR;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler(IApiClient apiClient) : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery query, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetApprenticeshipVacancyApiResponse>(new GetApprenticeshipVacancyApiRequest(query.VacancyReference, query.CandidateId));

            if (response == null)
            {
                throw new ResourceNotFoundException();
            }

            return new GetApprenticeshipVacancyQueryResult
            {
                Vacancy = response
            };
        }
    }
}
