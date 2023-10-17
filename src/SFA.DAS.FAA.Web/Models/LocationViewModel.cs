using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models;

public class LocationViewModel
{
    public LocationViewModel(List<string>? selectedRouteIds)
    {
        SelectedRouteIds = selectedRouteIds;
        RouteData = GetRouteData();
    }
    public List<string>? SelectedRouteIds { get; set; }
    public Dictionary<string,string> RouteData { get; set; }

    private Dictionary<string,string> GetRouteData()
    {
        var result = new Dictionary<string, string>();

        if (SelectedRouteIds is null)
        {
            return result;
        }
        for (var i = 0; i < SelectedRouteIds.Count; i++)
        {
            var selectedRouteId = SelectedRouteIds[i];
            result.Add($"routeIds[{i}]", selectedRouteId.ToString());
        }

        return result;
    }
}