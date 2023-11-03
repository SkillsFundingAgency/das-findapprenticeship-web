using SFA.DAS.FAA.Domain.LocationsBySearch;

namespace SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
public class GetLocationsBySearchQueryResult
{
    public IEnumerable<GetLocationsBySearchApiResponse.LocationItem> LocationItems
    {
        get;
        set;
    } = new List<GetLocationsBySearchApiResponse.LocationItem>();
}