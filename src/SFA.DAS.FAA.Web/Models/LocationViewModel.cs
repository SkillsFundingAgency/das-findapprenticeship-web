namespace SFA.DAS.FAA.Web.Models;

public class LocationViewModel : ViewModelBase
{
    public bool? CityOrPostcodeSelected { get; set; }
    public bool? AllOfEnglandSelected { get; set; }
    public string? CityOrPostcode { get; set; }
    public int? Distance { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public Dictionary<string, string> RouteData { get => GetRouteData(); }

    private Dictionary<string, string> GetRouteData()
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