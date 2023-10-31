using SFA.DAS.FAA.Domain.LocationsBySearch;

namespace SFA.DAS.FAA.Web.Models;

public class LocationsBySearchViewModel
{
    public List<LocationBySearchViewModel>? Locations { get; set; }

    public static implicit operator LocationsBySearchViewModel(List<GetLocationsBySearchApiResponse.LocationItem> source)
    {
        return new LocationsBySearchViewModel
        {
            Locations = source.Select(c=>(LocationBySearchViewModel)c).ToList()
        };
    }
}

public class LocationBySearchViewModel 
{
    public string? Name { get; set; }

    public static implicit operator LocationBySearchViewModel(Domain.LocationsBySearch.GetLocationsBySearchApiResponse.LocationItem source)
    {
        return new LocationBySearchViewModel
        {
            Name = source.Name
        };
    }
}

