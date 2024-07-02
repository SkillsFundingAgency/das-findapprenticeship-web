using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers;

public class SearchApprenticeshipsController(
    IMediator mediator, 
    IDateTimeService dateTimeService, 
    IOptions<Domain.Configuration.FindAnApprenticeship> faaConfiguration, 
    ICacheStorageService cacheStorageService, 
    SearchModelValidator searchModelValidator,
    GetSearchResultsRequestValidator searchRequestValidator) : Controller
{
    [Route("apprenticeshipsearch", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index(SearchModel model, [FromQuery] int? search = null)
    {
        var validationResult = await searchModelValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var validationFailure in validationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }
            return View(new SearchApprenticeshipsViewModel
            {
                WhatSearchTerm = model.WhatSearchTerm,
                WhereSearchTerm = model.WhereSearchTerm
            });
        }
        
        var result = await mediator.Send(new GetSearchApprenticeshipsIndexQuery
        {
            LocationSearchTerm = model.WhereSearchTerm
        });

        if (result is { LocationSearched: true, Location: null })
        {
            ModelState.AddModelError(nameof(SearchApprenticeshipsViewModel.WhereSearchTerm), "We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
        }
        else if (result.LocationSearched && result.Location != null)
        {
            return RedirectToRoute(RouteNames.SearchResults, new { location = result.Location.LocationName, distance = "10", searchTerm = model.WhatSearchTerm });
        }
        else if (search == 1)
        {
            return RedirectToRoute(RouteNames.SearchResults, new { searchTerm = model.WhatSearchTerm });
        }

        var viewModel = (SearchApprenticeshipsViewModel)result;
        viewModel.ShowAccountCreatedBanner =
            await NotificationBannerService.ShowAccountCreatedBanner(cacheStorageService,
                $"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}");

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
                var locationResult = await mediator.Send(new GetBrowseByInterestsLocationQuery { LocationSearchTerm = model.SearchTerm });

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

    [Route("apprenticeships", Name = RouteNames.SearchResults)]
    public async Task<IActionResult> SearchResults([FromQuery] GetSearchResultsRequest request)
    {
        var validationResult = await searchRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            foreach (var validationFailure in validationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }
            return View(new SearchResultsViewModel
            {
                SearchTerm = request.SearchTerm,
                Location = request.Location
            });
        }
        
        var validDistanceValues = new List<int> { 2, 5, 10, 15, 20, 30, 40 };
        if (request.Distance <= 0)
        {
            request.Distance = null;
        }
        else if (request.Distance.HasValue && !validDistanceValues.Contains((int)request.Distance))
        {
            request.Distance = 10;
        }
        else if (request.PageNumber <= 0)
        {
            request.PageNumber = 1;
        }
        
        var result = await mediator.Send(new GetSearchResultsQuery
        {
            Location = request.Location,
            SelectedRouteIds = request.RouteIds,
            SelectedLevelIds = request.LevelIds,
            Distance = request.Distance,
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = 10,
            Sort = request.Sort,
            DisabilityConfident = request.DisabilityConfident,
            CandidateId = User.Claims.CandidateId().Equals(null) ? null
                : User.Claims.CandidateId()!.ToString()
        });

        if (result.VacancyReference != null)
        {
            return RedirectToRoute(RouteNames.Vacancies, new { result.VacancyReference });
        }

        var filterUrl = FilterBuilder.BuildFullQueryString(request, Url);

        var viewmodel = (SearchResultsViewModel)result;
        viewmodel.SelectedRouteIds = request.RouteIds;
        viewmodel.NationalSearch = request.Location == null;
        viewmodel.Location = request.Location;
        viewmodel.Distance = request.Distance;
        viewmodel.SearchTerm = request.SearchTerm;
        viewmodel.Vacancies = result.Vacancies.Count != 0
            ? result.Vacancies.Select(c => new VacanciesViewModel().MapToViewModel(dateTimeService, c)).ToList()
            : [];
        viewmodel.MapData = result.Vacancies.Count != 0
            ? result.Vacancies.Select(c => new ApprenticeshipMapData().MapToViewModel(dateTimeService, c)).ToList()
            : [];
        viewmodel.MapId = faaConfiguration.Value.GoogleMapsId;
        viewmodel.SelectedRoutes = request.RouteIds != null ? result.Routes.Where(c => request.RouteIds.Contains(c.Id.ToString())).Select(c => c.Name).ToList() : [];
        viewmodel.DisabilityConfident = request.DisabilityConfident;
        viewmodel.PaginationViewModel = new PaginationViewModel(result.PageNumber, result.TotalPages, filterUrl);
        foreach (var route in viewmodel.Routes.Where(route => request.RouteIds != null && request.RouteIds!.Contains(route.Id.ToString())))
        {
            route.Selected = true;
        }
        foreach (var level in viewmodel.Levels.Where(level => request.LevelIds != null && request.LevelIds!.Contains(level.Id.ToString())))
        {
            level.Selected = true;
        }
        var filterChoices = PopulateFilterChoices(viewmodel.Routes, viewmodel.Levels);
        viewmodel.FilterChoices = filterChoices;
        viewmodel.SelectedLevelCount = request.LevelIds?.Count ?? 0;
        viewmodel.SelectedRouteCount = request.RouteIds?.Count ?? 0;
        viewmodel.SelectedFilters = FilterBuilder.Build(request, Url, filterChoices);
        viewmodel.ClearSelectedFiltersLink = Url.RouteUrl(RouteNames.SearchResults)!;
        viewmodel.ShowAccountCreatedBanner =
            await NotificationBannerService.ShowAccountCreatedBanner(cacheStorageService,
                $"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}");
        viewmodel.NoSearchResultsByUnknownLocation = !string.IsNullOrEmpty(request.Location) && result.Location == null;
        viewmodel.PageTitle = GetPageTitle(viewmodel);

        return View(viewmodel);
    }

    private static SearchApprenticeshipFilterChoices PopulateFilterChoices(IEnumerable<RouteViewModel> categories, IEnumerable<LevelViewModel> levels)
        => new()
        {
            JobCategoryChecklistDetails = new ChecklistDetails
            {
                Title = "RouteIds",
                QueryStringParameterName = "routeIds",
                Lookups = categories.OrderBy(x => x.Name).Select(category => new ChecklistLookup(category.Name, category.Id.ToString(), null, category.Selected)).ToList()
            },
            CourseLevelsChecklistDetails = new ChecklistDetails
            {
                Title = "LevelIds",
                QueryStringParameterName = "levelIds",
                Lookups = levels.OrderBy(x => x.Id).Select(level => new ChecklistLookup($"Level {level.Id}", level.Id.ToString(), $"Equal to {level.Name}", level.Selected)).ToList()
            }
        };

    private static string GetPageTitle(SearchResultsViewModel model)
    {
        if (model.Total == 0 || model.NoSearchResultsByUnknownLocation)
            return "No apprenticeships found";
        return model.Total switch
        {
            <= 10 => "Apprenticeships found",
            _ =>
                $"Apprenticeships found (page {model.PaginationViewModel.CurrentPage} of {model.PaginationViewModel.TotalPages})"
        };
    }
}