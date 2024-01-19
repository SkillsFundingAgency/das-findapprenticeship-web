using SFA.DAS.FAA.Domain.BrowseByInterests;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class RouteViewModel
{
    public static implicit operator RouteViewModel(RouteResponse route)
    {
        return new RouteViewModel
        {
            Selected = false,
            Name = route.Name,
            Id = route.Id
        };
    }
    public bool Selected { get; set; }
    public string Name { get; set; } = null!;
    public int Id { get; set; }
}