using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using MediatR;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers;

public class SearchApprenticeshipsController(IMediator mediator, IDateTimeService dateTimeService) : Controller
{
    [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index([FromQuery]string? whereSearchTerm = null, [FromQuery]string? whatSearchTerm = null, [FromQuery]int? search = null)
    {
        var result = await mediator.Send(new GetSearchApprenticeshipsIndexQuery
        {
            LocationSearchTerm = whereSearchTerm
        });

        if (result is { LocationSearched: true, Location: null })
        {
            ModelState.AddModelError(nameof(SearchApprenticeshipsViewModel.WhereSearchTerm), "We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
        }
        else if( result.LocationSearched && result.Location !=null)
        {
            return RedirectToRoute(RouteNames.SearchResults, new { location = result.Location.LocationName, distance = "10"});
        }
        else if(search == 1)
        {
            return RedirectToRoute(RouteNames.SearchResults);
        }
        
        var viewModel = (SearchApprenticeshipsViewModel)result;
        
        return View(viewModel);
    }

    [Route("browse-by-interests", Name = RouteNames.BrowseByInterests)]
    public async Task<IActionResult> BrowseByInterests([FromQuery] List<string>? routeIds = null)
    {
        var result = await mediator.Send(new GetBrowseByInterestsQuery());

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
            var result = await mediator.Send(new GetBrowseByInterestsQuery());

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
                var locationResult = await mediator.Send(new GetBrowseByInterestsLocationQuery{ LocationSearchTerm = model.SearchTerm });

                if (locationResult.Location == null)
                {
                    ModelState.AddModelError(nameof(LocationViewModel.SearchTerm), "We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return RedirectToRoute(RouteNames.SearchResults, new { routeIds = model.SelectedRouteIds, location = (model.NationalSearch == null || model.NationalSearch == false) ? model.SearchTerm : null, distance = model.Distance });

    }

    [Route("search-results", Name = RouteNames.SearchResults)]
    public async Task<IActionResult> SearchResults([FromQuery] List<string>? routeIds, [FromQuery] string? location, [FromQuery] int? distance, [FromQuery]string? searchTerm)
    {
        var result = await mediator.Send(new GetSearchResultsQuery
        {
            Location = location,
            SelectedRouteIds = routeIds,
            Distance = distance,
            SearchTerm = searchTerm
        });

        var viewmodel = (SearchResultsViewModel)result;
        viewmodel.SelectedRouteIds = routeIds;
        viewmodel.NationalSearch = (location == null);
        viewmodel.Location = location;
        viewmodel.Distance = distance;
        viewmodel.Vacancies = result.Vacancies.Any()
            ? result.Vacancies.Select(c => new VacanciesViewModel().MapToViewModel(dateTimeService, c)).ToList()
            : new List<VacanciesViewModel>();
        viewmodel.SelectedRoutes =
            routeIds != null ? result.Routes.Where(c => routeIds.Contains(c.Id.ToString())).Select(c => c.Name).ToList() : new List<string>();
        
        foreach (var route in viewmodel.Routes.Where(route => routeIds!.Contains(route.Id.ToString())))
        {
            route.Selected = true;
        }

        return View(viewmodel);
    }
}