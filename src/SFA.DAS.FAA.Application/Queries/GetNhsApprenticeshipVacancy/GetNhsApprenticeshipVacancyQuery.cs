using MediatR;

namespace SFA.DAS.FAA.Application.Queries.GetNhsApprenticeshipVacancy
{
    public record GetNhsApprenticeshipVacancyQuery(string VacancyReference)
        : IRequest<GetNhsApprenticeshipVacancyQueryResult>;
}