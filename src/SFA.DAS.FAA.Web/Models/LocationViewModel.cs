using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models;

public class LocationViewModel
{
    public LocationViewModel(List<string>? selectedRouteIds)
    {
        SelectedRouteIds = selectedRouteIds;
        BackUrl = CreateBackUrl();
    }
    public List<string>? SelectedRouteIds { get; set; }
    public string BackUrl { get; set; } = null!;

    private string CreateBackUrl()
    {
        if (SelectedRouteIds == null) { return string.Empty; }
        var backUrl = RouteNames.BrowseByInterests + "?RouteIds=" + string.Join("&RouteIds=", SelectedRouteIds.ToArray());
        return backUrl;
    }
}