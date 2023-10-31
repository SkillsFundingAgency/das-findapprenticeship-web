using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using MediatR;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Application.Queries.GetGeoPoint;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;

namespace SFA.DAS.FAA.Web.Controllers;

public class SearchApprenticeshipsController : Controller
{
    private readonly IMediator _mediator;

    public SearchApprenticeshipsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetSearchApprenticeshipsIndexQuery());

        var viewModel = (SearchApprenticeshipsViewModel)result;

        return View(viewModel);
    }

    [Route("browse-by-interests", Name = RouteNames.BrowseByInterests)]
    public async Task<IActionResult> BrowseByInterests([FromQuery] List<string>? routeIds = null)
    {
        var result = await _mediator.Send(new GetBrowseByInterestsQuery());

        var viewModel = (BrowseByInterestViewModel)result;

        viewModel.AllocateRouteGroup(routeIds);

        return View(viewModel);
    }

    [HttpPost]
    [Route("browse-by-interests", Name = RouteNames.BrowseByInterests)]
    public async Task<IActionResult> BrowseByInterests(BrowseByInterestRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            //TODO - no coverage
            var result = await _mediator.Send(new GetBrowseByInterestsQuery());

            var viewModel = (BrowseByInterestViewModel)result;
            
            viewModel.AllocateRouteGroup();

            return View(viewModel);
        }

        return RedirectToRoute(RouteNames.Location, new { RouteIds = model.SelectedRouteIds });
    }

    [Route("location", Name = RouteNames.Location)]
    public IActionResult Location([FromQuery] List<string>? routeIds)
    {
        var viewModel = new LocationViewModel() { SelectedRouteIds = routeIds };
        return View(viewModel);
    }

    [HttpPost]
    [Route("location", Name = RouteNames.Location)]
    public async Task<IActionResult> Location([FromQuery] List<string>? routeIds, LocationViewModel model)
    {
        model.SelectedRouteIds = routeIds;

        if (model.NationalSearch == false)
        {
            if (string.IsNullOrEmpty(model.SearchTerm))
            {
                ModelState.AddModelError(nameof(LocationViewModel.SearchTerm), "Enter a city or postcode");    
            }
            else
            {
                var locationResult = await _mediator.Send(new GetLocationsBySearchQuery { SearchTerm = model.SearchTerm });

                if (!locationResult.LocationItems.Any())
                {
                    ModelState.AddModelError(nameof(LocationViewModel.SearchTerm), "We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return RedirectToRoute(RouteNames.SearchResults, new { routeIds = model.SelectedRouteIds, location = (model.NationalSearch == null || model.NationalSearch == false) ? model.CityOrPostcode : null });

    }

    [Route("search-results", Name = RouteNames.SearchResults)]
    public async Task<IActionResult> SearchResults([FromQuery] List<string>? routeIds, [FromQuery] string? location)
    {
        var viewmodel = new SearchResultsViewModel()
        {
            SelectedRouteIds = routeIds,
            NationalSearch = (location == null),
            location = location
        };

        return View(viewmodel);
    }
}