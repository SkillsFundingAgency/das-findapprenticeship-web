namespace SFA.DAS.FAA.Web.Models;

public class LocationsBySearchViewModel
{
    public List<LocationBySearchViewModel> Locations { get; set; }
}

public class LocationBySearchViewModel 
{
    public string Name { get; set; }

    public static implicit operator LocationBySearchViewModel(Domain.LocationsBySearch.GetLocationsBySearchApiResponse.LocationItem source)
    {
        return new LocationBySearchViewModel
        {
            Name = source.Name
        };
    }
}

