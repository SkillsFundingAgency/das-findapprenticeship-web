using MediatR;

namespace SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
public class GetLocationsBySearchQuery : IRequest<GetLocationsBySearchQueryResult>
{
    public string SearchTerm { get; set; } = null!;
}
