using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
public class GetLocationsByIndexSearchQuery : IRequest<GetLocationsByIndexSearchQueryResult>
{
    public string SearchTerm { get; set; } = null!;
}
