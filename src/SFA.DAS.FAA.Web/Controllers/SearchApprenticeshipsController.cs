using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using MediatR;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.GovUK.Auth.Services;

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
    public async Task<IActionResult> SearchResults([FromQuery] GetSearchResultsRequest request)
    {
        var filterUrl = FilterBuilder.BuildFullQueryString(request, Url);

        var result = await mediator.Send(new GetSearchResultsQuery
        {
            Location = request.Location,
            SelectedRouteIds = request.RouteIds,
            SelectedLevelIds = request.LevelIds,
            Distance = request.Distance,
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
            Sort = request.Sort,
            DisabilityConfident = request.DisabilityConfident
        });

        var viewmodel = (SearchResultsViewModel)result;
        viewmodel.SelectedRouteIds = request.RouteIds;
        viewmodel.NationalSearch = request.Location == null;
        viewmodel.Location = request.Location;
        viewmodel.Distance = request.Distance;
        viewmodel.Vacancies = result.Vacancies.Count != 0
            ? result.Vacancies.Select(c => new VacanciesViewModel().MapToViewModel(dateTimeService, c)).ToList()
            : [];
        viewmodel.SelectedRoutes = request.RouteIds != null ? result.Routes.Where(c => request.RouteIds.Contains(c.Id.ToString())).Select(c => c.Name).ToList() : [];
        viewmodel.DisabilityConfident = request.DisabilityConfident;
        viewmodel.PaginationViewModel = new PaginationViewModel(result.PageNumber, result.PageSize, result.TotalPages, filterUrl);

        foreach (var route in viewmodel.Routes.Where(route => request.RouteIds != null && request.RouteIds!.Contains(route.Id.ToString())))
        {
            route.Selected = true;
        }
        foreach (var level in viewmodel.Levels.Where(level => request.LevelIds != null && request.LevelIds!.Contains(level.Id.ToString())))
        {
            level.Selected = true;
        }
        var filterChoices = PopulateFilterChoices(viewmodel.Routes, viewmodel.Levels);
        viewmodel.SelectedLevelCount = request.LevelIds?.Count ?? 0;
        viewmodel.SelectedRouteCount = request.RouteIds?.Count ?? 0;
        viewmodel.SelectedFilters = FilterBuilder.Build(request, Url, filterChoices);

        return View(viewmodel);
    }
    private static SearchApprenticeshipFilterChoices PopulateFilterChoices(IEnumerable<RouteViewModel> categories, IEnumerable<LevelViewModel> levels)
                Lookups = categories.OrderBy(x => x.Name).Select(category => new ChecklistLookup(category.Name, category.Id.ToString(), null, category.Selected)).ToList()
            },
            CourseLevelsChecklistDetails = new ChecklistDetails
            {
                Title = "LevelIds",
                QueryStringParameterName = "levelIds",
                Lookups = levels.OrderBy(x => x.Id).Select(level => new ChecklistLookup($"Level {level.Id}", level.Id.ToString(), $"Equal to {level.Name}", level.Selected)).ToList()
}