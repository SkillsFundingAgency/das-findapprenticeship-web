using System.ComponentModel.DataAnnotations;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;

namespace SFA.DAS.FAA.Web.Models;

public class BrowseByInterestViewModel : ViewModelBase
{
    public List<RouteViewModel> Routes { get; set; }

    public static implicit operator BrowseByInterestViewModel(GetBrowseByInterestsResult source)
    {
        return new BrowseByInterestViewModel()
        {
            Routes = source.Routes.Select(r => new RouteViewModel
            {
                Selected = false,
                Route = r.Route,
                Id = Guid.NewGuid()
            }).ToList()
        };
    }
    public class RouteViewModel
    {
        public bool Selected { get; set; }
        public string Route { get; set; }
        public Guid Id { get; set; }
    }
}