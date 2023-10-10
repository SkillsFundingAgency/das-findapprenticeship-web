using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;

namespace SFA.DAS.FAA.Web.Models;

public class BrowseByInterestRequestViewModel : BrowseByInterestViewModel
{
    [FromForm]
    [Required(ErrorMessage = "Select at least one job category you're interested in")]
    public List<int>? SelectedRouteIds { get; set; }
}

public class BrowseByInterestViewModel : ViewModelBase
{
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
        return new BrowseByInterestViewModel()
        {
            Routes = source.Routes.Select(r => new RouteViewModel
            {
                Selected = false,
                Route = r.Route,
                Id = r.Id
            }).ToList()
        };
    }
    public class RouteViewModel
    {
        public bool Selected { get; set; }
        public string Route { get; set; }
        public int Id { get; set; }
    }

    public class RouteObject
    {
        public string RouteId { get; set; }
        public string RouteName { get; set; }
        public string DisplayText { get; set; }
        public string HintText { get; set; }
    }

    

    public void AllocateRouteGroup()
    {
        foreach (var route in Routes)
        {
            switch (route.Id.ToString())
            {
                case "1":
                    var agriculture = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Farmer, horticulture, vet and similar"
                    };
                    AgricultureEnvironmentalAndAnimalCareDictionary.Add(route.Id.ToString(), agriculture);
                    break;

                case "2":
                    var businessAdministration = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Administrator, project manager, human resources and similar"
                    };
                    BusinessSalesAndLegalDictionary.Add(route.Id.ToString(), businessAdministration);
                    break;

                case "14":
                    var salesMarkerting = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = "Sales and marketing",
                        HintText = "Sales manager, digital marketer and similar"
                    };
                    BusinessSalesAndLegalDictionary.Add(route.Id.ToString(), salesMarkerting);
                    break;

                case "12":
                    var legalFinanceAccounting = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Financial advisor, payroll assistant, solicitor and similar"
                    };
                    BusinessSalesAndLegalDictionary.Add(route.Id.ToString(), legalFinanceAccounting);
                    break;

                case "10":
                    var hairBeauty = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Hairdresser, barber, holistic therapist and similar"
                    };
                    CareHealthAndScienceDictionary.Add(route.Id.ToString(), hairBeauty);
                    break;

                case "3":
                    var careServices = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Social worker, play therapist, adult carer and similar"
                    };
                    CareHealthAndScienceDictionary.Add(route.Id.ToString(), careServices);
                    break;

                case "11":
                    var healthScience = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Ambulance worker, sports coach, clinical scientist and similar"
                    };
                    CareHealthAndScienceDictionary.Add(route.Id.ToString(), healthScience);
                    break;

                case "4":
                    var cateringHospitality = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Chef, baker, hospitality team member and similar"
                    };
                    CateringAndHospitalityDictionary.Add(route.Id.ToString(), cateringHospitality);
                    break;

                case "5":
                    var construction = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Bricklayer, smart home technician, architect and similar"
                    };
                    ConstructionEngineeringAndBuildingsDictionary.Add(route.Id.ToString(), construction);
                    break;

                case "9":
                    var engineeringManufacturing = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Autocare technician, welder, rail engineer and similar"
                    };
                    ConstructionEngineeringAndBuildingsDictionary.Add(route.Id.ToString(), engineeringManufacturing);
                    break;

                case "6":
                    var creativeDesign = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Music technologist, camera technician, graphic designer and similar"
                    };
                    CreativeAndDesignDictionary.Add(route.Id.ToString(), creativeDesign);
                    break;

                case "7":
                    var digital = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Cyber security officer, software engineer, data scientist and similar"
                    };
                    DigitalDictionary.Add(route.Id.ToString(), digital);
                    break;

                case "8":
                    var education = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = "Education and early years",
                        HintText = "Teaching assistant, early years educator and similar"
                    };
                    EducationAndEarlyYearsDictionary.Add(route.Id.ToString(), education);
                    break;

                case "13":
                    var protectiveServices = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = "Protective services (emergency and uniformed services)",
                        HintText = "Security, firefighter, coastguard and similar"
                    };
                    ProtectiveServicesDictionary.Add(route.Id.ToString(), protectiveServices);
                    break;

                case "15":
                    var transportLogistics = new RouteObject
                    {
                        RouteId = route.Id.ToString(),
                        RouteName = route.Route,
                        DisplayText = route.Route,
                        HintText = "Aviation ground handler, train driver, fleet manager and similar"
                    };
                    TransportAndLogisticsDictionary.Add(route.Id.ToString(), transportLogistics);
                    break;
            }

        }
    }

}

