using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

public class GetIndexLocationQuery : IRequest<GetIndexLocationQueryResult>
{
    public string LocationSearchTerm { get; set; }
}