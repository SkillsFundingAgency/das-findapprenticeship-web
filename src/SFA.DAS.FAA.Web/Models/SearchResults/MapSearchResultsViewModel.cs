using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class MapSearchResultsViewModel
{
    public List<ApprenticeshipMapData> ApprenticeshipMapData { get; set; }
    public LocationViewModel SearchedLocation { get; set; }
}

public class LocationViewModel
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string LocationName { get; set; }
    
    public static implicit operator LocationViewModel(Location? source)
    {
        if (source == null)
        {
            return new LocationViewModel();
        }

        return new LocationViewModel
        {
            Lat = source.Lat,
            LocationName = source.LocationName,
            Lon = source.Lon
        };
    }
}