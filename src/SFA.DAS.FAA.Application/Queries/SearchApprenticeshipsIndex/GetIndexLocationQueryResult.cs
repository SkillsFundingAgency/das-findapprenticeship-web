using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;
using Location = SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex.Location;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

public class GetIndexLocationQueryResult
{
    public Location? Location { get; set; }
}