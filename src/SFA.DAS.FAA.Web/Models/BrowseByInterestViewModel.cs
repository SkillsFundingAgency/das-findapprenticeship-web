using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.Models;

public class BrowseByInterestViewModel
{
    public List<int>? SelectedRouteIds { get; set; }
    public List<RouteViewModel> Routes { get; set; } = new();
    public Dictionary<string, RouteObject> AgricultureEnvironmentalAndAnimalCareDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> BusinessSalesAndLegalDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> CareHealthAndScienceDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> CateringAndHospitalityDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> ConstructionEngineeringAndBuildingsDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> CreativeAndDesignDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> DigitalDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> EducationAndEarlyYearsDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> ProtectiveServicesDictionary { get; set; } = new();
    public Dictionary<string, RouteObject> TransportAndLogisticsDictionary { get; set; } = new();

    public static implicit operator BrowseByInterestViewModel(GetBrowseByInterestsResult source)
    {
        return new BrowseByInterestViewModel
        {
            Routes = source.Routes.Select(r => (RouteViewModel)r).ToList()
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
            switch (route.Id.ToString())
            {
                case "1":
                    var agriculture = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Farmer, horticulture, vet and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    AgricultureEnvironmentalAndAnimalCareDictionary.Add(route.Id.ToString(), agriculture);
                    break;

                case "2":
                    var businessAdministration = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Administrator, project manager, human resources and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    BusinessSalesAndLegalDictionary.Add(route.Id.ToString(), businessAdministration);
                    break;

                case "14":
                    var salesMarketing = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = "Sales and marketing",
                        HintText = "Sales manager, digital marketer and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    BusinessSalesAndLegalDictionary.Add(route.Id.ToString(), salesMarketing);
                    break;

                case "12":
                    var legalFinanceAccounting = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Financial advisor, payroll assistant, solicitor and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    BusinessSalesAndLegalDictionary.Add(route.Id.ToString(), legalFinanceAccounting);
                    break;

                case "10":
                    var hairBeauty = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Hairdresser, barber, holistic therapist and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    CareHealthAndScienceDictionary.Add(route.Id.ToString(), hairBeauty);
                    break;

                case "3":
                    var careServices = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Social worker, play therapist, adult carer and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    CareHealthAndScienceDictionary.Add(route.Id.ToString(), careServices);
                    break;

                case "11":
                    var healthScience = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Ambulance worker, sports coach, clinical scientist and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    CareHealthAndScienceDictionary.Add(route.Id.ToString(), healthScience);
                    break;

                case "4":
                    var cateringHospitality = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Chef, baker, hospitality team member and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    CateringAndHospitalityDictionary.Add(route.Id.ToString(), cateringHospitality);
                    break;

                case "5":
                    var construction = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Bricklayer, smart home technician, architect and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    ConstructionEngineeringAndBuildingsDictionary.Add(route.Id.ToString(), construction);
                    break;

                case "9":
                    var engineeringManufacturing = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Autocare technician, welder, rail engineer and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    ConstructionEngineeringAndBuildingsDictionary.Add(route.Id.ToString(), engineeringManufacturing);
                    break;

                case "6":
                    var creativeDesign = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Music technologist, camera technician, graphic designer and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    CreativeAndDesignDictionary.Add(route.Id.ToString(), creativeDesign);
                    break;

                case "7":
                    var digital = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Cyber security officer, software engineer, data scientist and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    DigitalDictionary.Add(route.Id.ToString(), digital);
                    break;

                case "8":
                    var education = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = "Education and early years",
                        HintText = "Teaching assistant, early years educator and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    EducationAndEarlyYearsDictionary.Add(route.Id.ToString(), education);
                    break;

                case "13":
                    var protectiveServices = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = "Protective services (emergency and uniformed services)",
                        HintText = "Security, firefighter, coastguard and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    ProtectiveServicesDictionary.Add(route.Id.ToString(), protectiveServices);
                    break;

                case "15":
                    var transportLogistics = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Name,
                        DisplayText = route.Name,
                        HintText = "Aviation ground handler, train driver, fleet manager and similar",
                        PreviouslySelected = SetPreviouslySelected(previouslySelectedValues, route.Id)
                    };
                    TransportAndLogisticsDictionary.Add(route.Id.ToString(), transportLogistics);
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

