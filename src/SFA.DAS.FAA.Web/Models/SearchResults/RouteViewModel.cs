using SFA.DAS.FAA.Domain.BrowseByInterests;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class RouteViewModel
{
    public static RouteViewModel ToViewModel(RouteResponse route)
    {
        var name = route.Id switch
        {
            5 => "Construction and building",
            14 => "Sales and marketing",
            _ => route.Name
        };
        
        return new RouteViewModel
        {
            Selected = false,
            Name = name,
            Id = route.Id
        };
    }
    
    public bool Selected { get; set; }
    public string Name { get; set; } = null!;
    public int Id { get; set; }
}