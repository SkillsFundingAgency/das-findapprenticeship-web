namespace SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
public class GetLocationsBySearchQueryResult
{
    public IEnumerable<Domain.LocationsBySearch.GetLocationsBySearchApiResponse.LocationItem> LocationItems { get; set; }
}
