using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models;

public class LocationViewModel
{
    public LocationViewModel(List<int>? selectedRouteIds)
    {
        SelectedRouteIds = selectedRouteIds;
        BackUrl = CreateBackUrl();
    }
    public List<int>? SelectedRouteIds { get; set; }
    public string BackUrl { get; set; } = null!;

    private string CreateBackUrl()
    {
        if (SelectedRouteIds == null) { return string.Empty; }
        //var backUrl = RouteNames.BrowseByInterests + "?" + SelectedRouteIds.;
        var routeIds = string.Empty;
        foreach (var routeId in SelectedRouteIds)
        {
            routeIds = routeIds + routeId.ToString() + ":";
        }
        //var backUrl = RouteNames.BrowseByInterests + "?routeIds=" + routeIds[0, routeIds.Length -1];
        //return backUrl;
    }
}