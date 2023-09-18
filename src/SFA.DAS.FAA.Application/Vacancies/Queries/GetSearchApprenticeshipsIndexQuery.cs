using System.Net;
using MediatR;

namespace SFA.DAS.FAA.Application.Vacancies.Queries;

public class GetSearchApprenticeshipsIndexQuery : IRequest<GetSearchApprenticeshipsIndexResult>
{
    public int Total { get; set; }
}
