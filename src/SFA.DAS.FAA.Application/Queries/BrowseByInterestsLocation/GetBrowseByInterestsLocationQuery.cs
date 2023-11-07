using MediatR;

namespace SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;

public class GetBrowseByInterestsLocationQuery : IRequest<GetBrowseByInterestsLocationQueryResult>
{
    public string LocationSearchTerm { get; set; }
}