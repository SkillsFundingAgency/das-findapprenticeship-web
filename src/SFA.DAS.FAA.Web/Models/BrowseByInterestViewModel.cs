using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.Models;

public class BrowseByInterestViewModel
{
    public List<int>? SelectedRouteIds { get; set; }
    public List<RouteViewModel> Routes { get; set; } = new();
    
    public Dictionary<string, RouteObject> BusinessFinanceAndPublicServicesDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> CreativeAndServiceIndustriesDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> EnvironmentAndAgricultureDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> PeopleAndCareDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> TechnicalAndEngineeringDictionary { get; set; } = new();

    public static BrowseByInterestViewModel ToViewModel(GetBrowseByInterestsResult source)
    {
        return new BrowseByInterestViewModel
        {
            Routes = source.Routes.Select(RouteViewModel.ToViewModel).ToList()
        };
    }

    public class RouteObject
    {
        public string RouteId { get; set; } = null!;
        public string RouteName { get; set; } = null!;
        public string DisplayText { get; set; } = null!;
        public string HintText { get; set; } = null!;
        public bool PreviouslySelected { get; set; }
    }

    public void AllocateRouteGroup(List<string>? previouslySelectedValues = null)
    {
        foreach (var route in Routes)
        {
            var routeObject = new RouteObject()
            {
                RouteId = route.Id.ToString(),
                RouteName = route.Name,
                DisplayText = route.Name,
                PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
            };
            
            switch (route.Id.ToString())
            {
                case "1":
                    routeObject.HintText = "e.g. farmer, gardener, vet";
                    EnvironmentAndAgricultureDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "2":
                    routeObject.HintText = "e.g. administrator, project manager, human resources";
                    BusinessFinanceAndPublicServicesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "3":
                    routeObject.HintText = "e.g. social worker, play therapist, adult carer";
                    PeopleAndCareDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "4":
                    routeObject.HintText = "e.g. chef, baker, hospitality staff";
                    CreativeAndServiceIndustriesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "5":
                    routeObject.DisplayText = "Construction and building";
                    routeObject.HintText = "e.g. bricklayer, carpenter, architect";
                    TechnicalAndEngineeringDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "6":
                    routeObject.HintText = "e.g. graphic designer, photographer, animator";
                    CreativeAndServiceIndustriesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "7":
                    routeObject.HintText = "e.g. software engineer, data scientist, cloud engineer";
                    TechnicalAndEngineeringDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "8":
                    routeObject.HintText = "e.g. teaching assistant, early years educator";
                    PeopleAndCareDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "9":
                    routeObject.HintText = "e.g. autocare technician, welder, rail engineer";
                    TechnicalAndEngineeringDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "10":
                    routeObject.HintText = "e.g. hairdresser, barber, holistic therapist";
                    CreativeAndServiceIndustriesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "11":
                    routeObject.HintText = "e.g. ambulance worker, sports coach, clinical scientist";
                    PeopleAndCareDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "12":
                    routeObject.HintText = "e.g. financial advisor, payroll assistant, solicitor";
                    BusinessFinanceAndPublicServicesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "13":
                    routeObject.HintText = "e.g. security, firefighter, coastguard";
                    BusinessFinanceAndPublicServicesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "14":
                    routeObject.HintText = "e.g. sales manager, digital marketer";
                    BusinessFinanceAndPublicServicesDictionary.Add(route.Id.ToString(), routeObject);
                    break;

                case "15":
                    routeObject.HintText = "e.g. aviation ground handler, train driver, fleet manager";
                    BusinessFinanceAndPublicServicesDictionary.Add(route.Id.ToString(), routeObject);
                    break;
            }
        }

    }

    private static bool SetPreviouslySelected(List<string>? previouslySelectedValues, int routeId)
    {
        if (previouslySelectedValues != null && previouslySelectedValues.Contains(routeId.ToString()))
        {
            return true;
        }
        return false;
    }

}

