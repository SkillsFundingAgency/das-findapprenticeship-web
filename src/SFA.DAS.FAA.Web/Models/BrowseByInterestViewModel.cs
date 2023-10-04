using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using StackExchange.Redis;

namespace SFA.DAS.FAA.Web.Models;

public class BrowseByInterestViewModel : ViewModelBase
{
    [Required(ErrorMessage = "Select at least one job category you're interested in")]
    public List<int> SelectedRouteIds { get; set; } = new List<int>();
    public List<RouteViewModel> Routes { get; set; }

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

    public class routeObject
    {
        public string routeId { get; set; }
        public string routeName { get; set; }
        public string displayText { get; set; }
        public string hintText { get; set; }
    }

    public Dictionary<string, routeObject> agriculutreEnvironmentalAndAnimalCareDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> businessSalesandLegalDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> careHealthAndScienceDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> cateringAndHospitalityDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> constructionEngineeringAndBuildingsDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> creativeAndDesignDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> digitalDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> educationAndEarlyYearsDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> protectiveServicesDictionary = new Dictionary<string, routeObject>();
    public Dictionary<string, routeObject> transportAndLogisticsDictionary = new Dictionary<string, routeObject>();

    public void allocateRouteGroup()
    {

        foreach (var route in Routes)
        {
            switch (route.Id.ToString())
            {
                case "1":
                    routeObject agriculture = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Farmer, horticulture, vet and similar"
                    };
                    agriculutreEnvironmentalAndAnimalCareDictionary.Add(route.Id.ToString(), agriculture);
                    break;

                case "2":
                    routeObject businessAdministration = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Administrator, project manager, human resources and similar"
                    };
                    businessSalesandLegalDictionary.Add(route.Id.ToString(), businessAdministration);
                    break;

                case "14":
                    routeObject salesMarkerting = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = "Sales and marketing",
                        hintText = "Sales manager, digital marketer and similar"
                    };
                    businessSalesandLegalDictionary.Add(route.Id.ToString(), salesMarkerting);
                    break;

                case "12":
                    routeObject legalFinanceAccounting = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Financial advisor, payroll assistant, solicitor and similar"
                    };
                    businessSalesandLegalDictionary.Add(route.Id.ToString(), legalFinanceAccounting);
                    break;

                case "10":
                    routeObject hairBeauty = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Hairdresser, barber, holistic therapist and similar"
                    };
                    careHealthAndScienceDictionary.Add(route.Id.ToString(), hairBeauty);
                    break;

                case "3":
                    routeObject careServices = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Social worker, play therapist, adult carer and similar"
                    };
                    careHealthAndScienceDictionary.Add(route.Id.ToString(), careServices);
                    break;

                case "11":
                    routeObject healthScience = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Ambulance worker, sports coach, clinical scientist and similar"
                    };
                    careHealthAndScienceDictionary.Add(route.Id.ToString(), healthScience);
                    break;

                case "4":
                    routeObject cateringHospitality = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Chef, baker, hospitality team member and similar"
                    };
                    cateringAndHospitalityDictionary.Add(route.Id.ToString(), cateringHospitality);
                    break;

                case "5":
                    routeObject construction = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Bricklayer, smart home technician, architect and similar"
                    };
                    constructionEngineeringAndBuildingsDictionary.Add(route.Id.ToString(), construction);
                    break;

                case "9":
                    routeObject engineeringManufacturing = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Autocare technician, welder, rail engineer and similar"
                    };
                    constructionEngineeringAndBuildingsDictionary.Add(route.Id.ToString(), engineeringManufacturing);
                    break;

                case "6":
                    routeObject creativeDesign = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Music technologist, camera technician, graphic designer and similar"
                    };
                    creativeAndDesignDictionary.Add(route.Id.ToString(), creativeDesign);
                    break;

                case "7":
                    routeObject digital = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Cyber security officer, software engineer, data scientist and similar"
                    };
                    digitalDictionary.Add(route.Id.ToString(), digital);
                    break;

                case "8":
                    routeObject education = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = "Education and early years",
                        hintText = "Teaching assistant, early years educator and similar"
                    };
                    educationAndEarlyYearsDictionary.Add(route.Id.ToString(), education);
                    break;

                case "13":
                    routeObject protectiveServices = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = "Protective services (emergency and uniformed services)",
                        hintText = "Security, firefighter, coastguard and similar"
                    };
                    protectiveServicesDictionary.Add(route.Id.ToString(), protectiveServices);
                    break;

                case "15":
                    routeObject transportLogistics = new routeObject()
                    {
                        routeId = route.Id.ToString(),
                        routeName = route.Route,
                        displayText = route.Route,
                        hintText = "Aviation ground handler, train driver, fleet manager and similar"
                    };
                    transportAndLogisticsDictionary.Add(route.Id.ToString(), transportLogistics);
                    break;
            }

        }
    }

}

