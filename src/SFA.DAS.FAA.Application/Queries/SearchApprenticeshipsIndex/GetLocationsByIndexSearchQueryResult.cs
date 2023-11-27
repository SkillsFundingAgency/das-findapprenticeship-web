using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
public class GetLocationsByIndexSearchQueryResult
{
    public IEnumerable<GetLocationsByIndexSearchApiResponse.LocationItem> LocationItems
    {
        get;
        set;
    } = new List<GetLocationsByIndexSearchApiResponse.LocationItem>();
}