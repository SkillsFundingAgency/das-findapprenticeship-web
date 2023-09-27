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
            Routes = (List<RouteViewModel>)source.Routes.Select(Route => new RouteViewModel()
            {
                Selected = false,
                Route = Route.Routes,
                Id = Guid.NewGuid()
            })
        };
    }
    public class RouteViewModel
    {
        public bool Selected { get; set; }
        public string Route { get; set; }
        public Guid Id { get; set; }
    }
}