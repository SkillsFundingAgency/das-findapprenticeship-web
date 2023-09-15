using System.Net;
using MediatR;

namespace SFA.DAS.FAA.Application.Vacancies.Queries;

public class GetVacanciesQuery : IRequest<GetVacanciesResult>
{
    public int Total { get; set; }
}
