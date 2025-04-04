using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.Commands.SavedSearches.PostSaveSearch;
using SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy;
using SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;
using Constants = SFA.DAS.FAA.Application.Constants.Constants;
using LocationViewModel = SFA.DAS.FAA.Web.Models.LocationViewModel;

namespace SFA.DAS.FAA.Web.Controllers;

public class SearchApprenticeshipsController(
    IMediator mediator, 
    IDateTimeService dateTimeService, 
    IOptions<Domain.Configuration.FindAnApprenticeship> faaConfiguration, 
    ICacheStorageService cacheStorageService, 
    SearchModelValidator searchModelValidator,
    GetSearchResultsRequestValidator searchRequestValidator,
    IDataProtectorService dataProtectorService,
    ILogger<SearchApprenticeshipsController> logger) : Controller
{
    [Route("")]
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
            LocationSearchTerm = model.WhereSearchTerm,
            CandidateId = User.Claims.CandidateId()
        });

        if (result is { LocationSearched: true, Location: null })
        {
            ModelState.AddModelError(nameof(SearchApprenticeshipsViewModel.WhereSearchTerm), "We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
        }
        else if (result is {LocationSearched: true, Location: not null})
        {
            return RedirectToRoute(RouteNames.SearchResults, new { location = result.Location.LocationName, distance = "10", searchTerm = model.WhatSearchTerm, sort = VacancySort.DistanceAsc });
        }
        else if (search == 1)
        {
            return RedirectToRoute(RouteNames.SearchResults, new { searchTerm = model.WhatSearchTerm, sort = "AgeAsc" });
        }

        var viewModel = (SearchApprenticeshipsViewModel)result;
        viewModel.SavedSearches = result.SavedSearches != null
            ? result.SavedSearches.Select(c => SavedSearchViewModel.From(c, result.Routes!, Url)).ToList()
            : [];
        viewModel.ShowAccountCreatedBanner = await NotificationBannerService.ShowAccountBanner(cacheStorageService, $"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}");
        viewModel.ShowApprenticeshipWeekBanner = dateTimeService.GetDateTime() < new DateTime(2025, 02, 17);
        var isAccountDeleted = TempData[CacheKeys.AccountDeleted] as string;
        if (isAccountDeleted is "true")
        {
            viewModel.ShowAccountDeletedBanner = true;
            TempData.Remove(CacheKeys.AccountDeleted);
        }

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
    public async Task<IActionResult> BrowseByInterests(BrowseByInterestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = await mediator.Send(new GetBrowseByInterestsQuery());

            var viewModel = (BrowseByInterestViewModel)result;
            
            viewModel.AllocateRouteGroup();

            return View(viewModel);
        }

        return RedirectToRoute(RouteNames.Location, new { routeIds = model.SelectedRouteIds });
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

        return RedirectToRoute(RouteNames.SearchResults, new
        {
            routeIds = model.SelectedRouteIds,
            location = model.NationalSearch is null or false ? model.SearchTerm : null,
            distance = model.Distance,
            routePath = Constants.SearchResultRoutePath.Location
        });
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
        
        if (request.PageNumber <= 0)
        {
            request.PageNumber = 1;
        }
        
        var result = await mediator.Send(new GetSearchResultsQuery
        {
            CandidateId = User.Claims.CandidateId().Equals(null) ? null : User.Claims.CandidateId()!.ToString(),
            DisabilityConfident = request.DisabilityConfident,
            Distance = request.Distance,
            ExcludeNational = request.ExcludeNational,
            Location = request.Location,
            PageNumber = request.PageNumber,
            PageSize = 10,
            SearchTerm = request.SearchTerm,
            SelectedLevelIds = request.LevelIds,
            SelectedRouteIds = request.RouteIds,
            SkipWageType = request.IncludeCompetitiveSalaryVacancies ? null : WageType.CompetitiveSalary,
            Sort = request.Sort,
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
        viewmodel.VacancyAdverts = result.VacancyAdverts.Count != 0
            ? result.VacancyAdverts.Select(c => VacancyAdvertViewModel.MapToViewModel(dateTimeService, c, result.CandidateDateOfBirth)).ToList()
            : [];
        viewmodel.MapData = result.VacancyAdverts.Count != 0
            ? result.VacancyAdverts.Select(c => ApprenticeshipMapData.MapToViewModel(dateTimeService, c, result.CandidateDateOfBirth)).ToList()
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
        viewmodel.ClearSelectedFiltersLink = Url.RouteUrl(RouteNames.SearchResults, new{sort = VacancySort.AgeAsc} )!;
        viewmodel.ShowAccountCreatedBanner =
            await NotificationBannerService.ShowAccountBanner(cacheStorageService,
                $"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}");
        viewmodel.NoSearchResultsByUnknownLocation = !string.IsNullOrEmpty(request.Location) && result.Location == null;
        viewmodel.PageTitle = GetPageTitle(viewmodel);

        viewmodel.PageBackLinkRoutePath = request.RoutePath;
        
        viewmodel.CompetitiveSalaryRoutePath = request.IncludeCompetitiveSalaryVacancies
            ? FilterBuilder.ReplaceQueryStringParam(filterUrl, "IncludeCompetitiveSalaryVacancies", "false")
            : FilterBuilder.ReplaceQueryStringParam(filterUrl, "IncludeCompetitiveSalaryVacancies", "true");

        viewmodel.PageBackLinkRoutePath = request.RoutePath;

        viewmodel.EncodedRequestData = dataProtectorService.EncodedData(JsonConvert.SerializeObject(request));
        viewmodel.SearchAlreadySaved = result.SearchAlreadySaved;
        viewmodel.ExcludeNational = request.ExcludeNational ?? false;
        
        return View(viewmodel);
    }

    [HttpGet]
    [Route("map-search-results", Name = RouteNames.MapSearchResults)]
    public async Task<IActionResult> MapSearchResults([FromQuery] GetSearchResultsRequest request)
    {
        var validDistanceValues = new List<int> { 2, 5, 10, 15, 20, 30, 40 };
        if (request.Distance <= 0)
        {
            request.Distance = null;
        }
        else if (request.Distance.HasValue && !validDistanceValues.Contains((int)request.Distance))
        {
            request.Distance = 10;
        }

        var result = await mediator.Send(new GetSearchResultsQuery
        {
            CandidateId = User.Claims.CandidateId().Equals(null) ? null : User.Claims.CandidateId()!.ToString(),
            DisabilityConfident = request.DisabilityConfident,
            Distance = request.Distance,
            ExcludeNational = request.ExcludeNational,
            Location = request.Location,
            PageNumber = 1,
            PageSize = 300,
            SearchTerm = request.SearchTerm,
            SelectedLevelIds = request.LevelIds,
            SelectedRouteIds = request.RouteIds,
            Sort = request.Sort,
        });
        
        var results = result
            .VacancyAdverts
            .Where(x => x is { Lat: not null and not 0, Lon: not null and not 0 })
            .Select(c => ApprenticeshipMapData.MapToViewModel(dateTimeService, c, result.CandidateDateOfBirth))
            .ToList();
        
        var model = new MapSearchResultsViewModel
        {
            ApprenticeshipMapData = results,
            SearchedLocation = result.Location
        };

        model.ApprenticeshipMapData = model.ApprenticeshipMapData.Where(x => x.Job.VacancyLocation is not "Recruiting nationally").ToList();
        
        return Json(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("search-results/save/{vacancyReference}", Name = RouteNames.SaveVacancyFromSearchResultsPage)]
    public async Task<IActionResult> SearchResultsSaveVacancy([FromRoute] string vacancyReference, [FromQuery] bool redirect = true)
    {
        await mediator.Send(new SaveVacancyCommand
        {
            VacancyReference = vacancyReference,
            CandidateId = (Guid)User.Claims.CandidateId()!
        });

        var redirectUrl = Request.Headers.Referer.FirstOrDefault() ?? Url.RouteUrl(RouteNames.SearchResults) ?? "/";

        return redirect
            ? Redirect(redirectUrl)
            : new JsonResult(StatusCodes.Status200OK);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("search-results/delete/{vacancyReference}", Name = RouteNames.DeleteSavedVacancyFromSearchResultsPage)]
    public async Task<IActionResult> SearchResultsDeleteSavedVacancy([FromRoute] string vacancyReference, [FromQuery] bool redirect = true)
    {
        await mediator.Send(new DeleteSavedVacancyCommand
        {
            VacancyReference = vacancyReference,
            CandidateId = (Guid)User.Claims.CandidateId()!
        });

        var redirectUrl = Request.Headers.Referer.FirstOrDefault() ?? Url.RouteUrl(RouteNames.SearchResults) ?? "/";

        return redirect
            ? Redirect(redirectUrl)
            : new JsonResult(StatusCodes.Status200OK);
    }
    
    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("apprenticeships/save-search", Name = RouteNames.SaveSearch)]
    public async Task<IActionResult> SaveSearch([FromForm] SaveSearchRequest request, [FromQuery] bool redirect = true)
    {
        var redirectUrl = Request.Headers.Referer.FirstOrDefault() ?? Url.RouteUrl(RouteNames.SearchResults) ?? "/";
        try
        {
            var criteria = DecodeSearchCriteria(request.Data);
            if (criteria is null)
            {
                return Redirect(redirectUrl);
            }

            var saveSearchId = Guid.NewGuid();

            await mediator.Send(new SaveSearchCommand
            {
                Id = saveSearchId,
                SearchTerm = criteria.SearchTerm,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                DisabilityConfident = criteria.DisabilityConfident,
                Distance = string.IsNullOrEmpty(criteria.Location) ? null : criteria.Distance,
                Location = criteria.Location,
                SelectedLevelIds = criteria.LevelIds,
                SelectedRouteIds = criteria.RouteIds,
                SortOrder = criteria.Sort,
                UnSubscribeToken = dataProtectorService.EncodedData(saveSearchId.ToString()),
                ExcludeNational = criteria.ExcludeNational
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "SaveSearch: Unable to decode search criteria data");
            return Redirect(redirectUrl);
        }
        
        return redirect
            ? Redirect(redirectUrl)
            : new JsonResult(StatusCodes.Status200OK);
    }

    private GetSearchResultsRequest? DecodeSearchCriteria(string? encodedData)
    {
        if (encodedData is null)
        {
            return null;
        }
        
        var data = dataProtectorService.DecodeData(encodedData);
        return data is null 
            ? null 
            : JsonConvert.DeserializeObject<GetSearchResultsRequest>(data);
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
            return "No results found";

        return model.Total switch
        {
            1 => $"{model.Total} results found",
            <= 10 => $"{model.Total} results found",
            _ =>
                $"{model.Total} results found (page {model.PaginationViewModel.CurrentPage} of {model.PaginationViewModel.TotalPages})"
        };
    }
}