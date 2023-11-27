using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

public class GetIndexLocationQuery : IRequest<GetBrowseByInterestsLocationQueryResult>
{
    public string LocationSearchTerm { get; set; }
}