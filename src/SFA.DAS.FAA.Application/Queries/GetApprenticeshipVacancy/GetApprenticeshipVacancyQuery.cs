using MediatR;

namespace SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQuery : IRequest<GetApprenticeshipVacancyQueryResult>
    {
        public required string VacancyReference { get; init; }
    }
}
