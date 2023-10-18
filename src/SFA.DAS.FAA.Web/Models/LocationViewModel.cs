using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models;

public class LocationViewModel : ViewModelBase
{
    [BindProperty]
    public bool? cityOrPostcodeSelected { get; set; }
    [BindProperty]
    public bool? allOfEngland { get; set; }
    [BindProperty]
    public string? cityOrPostcode { get; set; }
    [BindProperty]
    public string distance { get; set; }



    public LocationViewModel(List<string>? selectedRouteIds = null)
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