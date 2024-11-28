using MediatR;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.GetNhsApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.GetNhsApprenticeshipVacancy
{
    public class GetNhsApprenticeshipVacancyQueryHandler(IApiClient apiClient) : IRequestHandler<GetNhsApprenticeshipVacancyQuery, GetNhsApprenticeshipVacancyQueryResult>
    {
        public async Task<GetNhsApprenticeshipVacancyQueryResult> Handle(GetNhsApprenticeshipVacancyQuery query, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetNhsApprenticeshipVacancyApiResponse>(new GetNhsApprenticeshipVacancyApiRequest(query.VacancyReference));

            if (response == null)
            {
                throw new ResourceNotFoundException();
            }

            return new GetNhsApprenticeshipVacancyQueryResult
            {
                Vacancy = response
            };
        }
    }
}