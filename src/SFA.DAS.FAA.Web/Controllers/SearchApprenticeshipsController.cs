using FluentValidation;
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
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using Constants = SFA.DAS.FAA.Application.Constants.Constants;
using LocationViewModel = SFA.DAS.FAA.Web.Models.LocationViewModel;

namespace SFA.DAS.FAA.Web.Controllers;

public class SearchApprenticeshipsController(
    IMediator mediator, 
    IDateTimeService dateTimeService, 
    IOptions<FindAnApprenticeship> faaConfiguration, 
    ICacheStorageService cacheStorageService, 
    IDataProtectorService dataProtectorService,
    ILogger<SearchApprenticeshipsController> logger) : Controller
{
    private const int PageSize = 10;

    [Route("")]
    [Route("apprenticeshipsearch", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public async Task<IActionResult> Index(
        [FromServices] IValidator<SearchModel> validator,
        SearchModel model,
        [FromQuery] int? search = null)
    {
        await validator.ValidateAndUpdateModelStateAsync(model, ModelState);
        if (!ModelState.IsValid)
        {
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
    public async Task<IActionResult> BrowseByInterests(
        [FromServices] IValidator<BrowseByInterestViewModel> validator,
        BrowseByInterestViewModel model)
    {
        await validator.ValidateAndUpdateModelStateAsync(model, ModelState);
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
    public async Task<IActionResult> Location(
        [FromServices] IValidator<LocationViewModel> validator,
        [FromQuery] List<string>? routeIds,
        LocationViewModel model)
    {
        await validator.ValidateAndUpdateModelStateAsync(model, ModelState);
        
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
    public async Task<IActionResult> SearchResults(
        [FromServices] IValidator<GetSearchResultsRequest> validator,
        [FromQuery] GetSearchResultsRequest request)
    {
        var validationResult = await validator.ValidateAndUpdateModelStateAsync(request, ModelState);
        if (!validationResult.IsValid)
        {
            return View(new SearchResultsViewModel
            {
                SearchTerm = request.SearchTerm,
                Location = request.Location
            });
        }

        // Normalize distance and page number
        var validDistances = new HashSet<int> { 2, 5, 10, 15, 20, 30, 40 };
        if (request.Distance <= 0)
        {
            request.Distance = null;
        }
        else if (request.Distance.HasValue && !validDistances.Contains(request.Distance.Value))
        {
            request.Distance = 10;
        }

        if (request.PageNumber <= 0)
        {
            request.PageNumber = 1;
        }

        // Prepare and send the query
        var candidateId = User.Claims.CandidateId();
        var getSearchResultsQuery = new GetSearchResultsQuery
        {
            ApprenticeshipTypes = request.ApprenticeshipTypes,
            CandidateId = candidateId == null ? null : candidateId.ToString(),
            DisabilityConfident = request.DisabilityConfident,
            Distance = request.Distance,
            ExcludeNational = request.ExcludeNational,
            Location = request.Location,
            PageNumber = request.PageNumber,
            PageSize = PageSize,
            SearchTerm = request.SearchTerm,
            SelectedLevelIds = request.LevelIds,
            SelectedRouteIds = request.RouteIds,
            SkipWageType = request.IncludeCompetitiveSalaryVacancies ? null : WageType.CompetitiveSalary,
            Sort = request.Sort,
        };

        var result = await mediator.Send(getSearchResultsQuery);

        // Redirect if a single vacancy reference is found
        if (!string.IsNullOrEmpty(result.VacancyReference))
        {
            return RedirectToRoute(RouteNames.Vacancies, new { result.VacancyReference });
        }

        // Build filter URL and map results to view model
        var filterUrl = FilterBuilder.BuildFullQueryString(request, Url);
        var viewModel = (SearchResultsViewModel)result;

        viewModel.SelectedRouteIds = request.RouteIds;
        viewModel.NationalSearch = string.IsNullOrEmpty(request.Location);
        viewModel.Location = request.Location;
        viewModel.Distance = request.Distance;
        viewModel.SearchTerm = request.SearchTerm;
        viewModel.VacancyAdverts = result.VacancyAdverts.Count > 0
            ? result.VacancyAdverts.Select(c => VacancyAdvertViewModel.MapToViewModel(dateTimeService, c, result.CandidateDateOfBirth)).ToList()
            : [];
        viewModel.MapData = result.VacancyAdverts.Count > 0
            ? result.VacancyAdverts.Select(c => ApprenticeshipMapData.MapToViewModel(dateTimeService, c, result.CandidateDateOfBirth)).ToList()
            : [];
        viewModel.MapId = faaConfiguration.Value.GoogleMapsId;
        viewModel.SelectedRoutes = request.RouteIds != null
            ? result.Routes.Where(c => request.RouteIds.Contains(c.Id.ToString())).Select(c => c.Name).ToList()
            : [];
        viewModel.DisabilityConfident = request.DisabilityConfident;
        viewModel.PaginationViewModel = new PaginationViewModel(result.PageNumber, result.TotalPages, filterUrl);

        // Mark selected routes and levels
        if (request.RouteIds != null)
        {
            foreach (var route in viewModel.Routes.Where(r => request.RouteIds.Contains(r.Id.ToString())))
            {
                route.Selected = true;
            }
        }
        if (request.LevelIds != null)
        {
            foreach (var level in viewModel.Levels.Where(l => request.LevelIds.Contains(l.Id.ToString())))
            {
                level.Selected = true;
            }
        }
        List<ApprenticeshipTypesViewModel> apprenticeshipTypesModels = [
            new()
            {
                HintText = "Introductory apprenticeships for young people, level 2",
                Id = 1,
                Name = "Foundation apprenticeship",
                Selected = request.ApprenticeshipTypes?.Contains(ApprenticeshipTypes.Foundation) ?? false,
                Value = nameof(ApprenticeshipTypes.Foundation)
            },
            new()
            {
                HintText = "Apprenticeships that qualify you for a job, levels 2 to 7",
                Id = 2,
                Name = "Apprenticeship",
                Selected = request.ApprenticeshipTypes?.Contains(ApprenticeshipTypes.Standard) ?? false,
                Value = nameof(ApprenticeshipTypes.Standard)
            }
        ];

        // Populate filter choices and selected filters
        var filterChoices = PopulateFilterChoices(viewModel.Routes, viewModel.Levels, apprenticeshipTypesModels);
        viewModel.FilterChoices = filterChoices;
        viewModel.SelectedLevelCount = request.LevelIds?.Count ?? 0;
        viewModel.SelectedRouteCount = request.RouteIds?.Count ?? 0;
        viewModel.SelectedApprenticeshipTypesCount = request.ApprenticeshipTypes?.Count ?? 0;
        viewModel.SelectedFilters = FilterBuilder.Build(request, Url, filterChoices);
        viewModel.ClearSelectedFiltersLink = Url.RouteUrl(RouteNames.SearchResults, new { sort = VacancySort.AgeAsc })!;

        // Banner and page title logic
        viewModel.ShowAccountCreatedBanner = await NotificationBannerService.ShowAccountBanner(
            cacheStorageService, $"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}");
        viewModel.NoSearchResultsByUnknownLocation = !string.IsNullOrEmpty(request.Location) && result.Location == null;
        viewModel.PageTitle = GetPageTitle(viewModel);

        // Back link and competitive salary toggle
        viewModel.PageBackLinkRoutePath = request.RoutePath;
        viewModel.CompetitiveSalaryRoutePath = FilterBuilder.ReplaceQueryStringParam(
            filterUrl, "IncludeCompetitiveSalaryVacancies", (!request.IncludeCompetitiveSalaryVacancies).ToString().ToLowerInvariant());

        // Encoded request data and other flags
        viewModel.EncodedRequestData = dataProtectorService.EncodedData(JsonConvert.SerializeObject(request));
        viewModel.SearchAlreadySaved = result.SearchAlreadySaved;
        viewModel.ExcludeNational = request.ExcludeNational ?? false;

        return View(viewModel);
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
            ApprenticeshipTypes = request.ApprenticeshipTypes,
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
    [Route("search-results/save/{vacancyId}", Name = RouteNames.SaveVacancyFromSearchResultsPage)]
    public async Task<IActionResult> SearchResultsSaveVacancy([FromRoute] string vacancyId, [FromQuery] bool redirect = true)
    {
        await mediator.Send(new SaveVacancyCommand
        {
            VacancyId = vacancyId,
            CandidateId = (Guid)User.Claims.CandidateId()!
        });

        var redirectUrl = Request.Headers.Referer.FirstOrDefault() ?? Url.RouteUrl(RouteNames.SearchResults) ?? "/";

        return redirect
            ? Redirect(redirectUrl)
            : new JsonResult(StatusCodes.Status200OK);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("search-results/delete/{vacancyId}", Name = RouteNames.DeleteSavedVacancyFromSearchResultsPage)]
    public async Task<IActionResult> SearchResultsDeleteSavedVacancy([FromRoute] string vacancyId, [FromQuery] bool redirect = true)
    {
        await mediator.Send(new DeleteSavedVacancyCommand
        {
            VacancyId = vacancyId,
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
                ApprenticeshipTypes = criteria.ApprenticeshipTypes,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                DisabilityConfident = criteria.DisabilityConfident,
                Distance = string.IsNullOrEmpty(criteria.Location) ? null : criteria.Distance,
                ExcludeNational = criteria.ExcludeNational,
                Id = saveSearchId,
                Location = criteria.Location,
                SearchTerm = criteria.SearchTerm,
                SelectedLevelIds = criteria.LevelIds,
                SelectedRouteIds = criteria.RouteIds,
                SortOrder = criteria.Sort,
                UnSubscribeToken = dataProtectorService.EncodedData(saveSearchId.ToString()),
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
    
    private static SearchApprenticeshipFilterChoices PopulateFilterChoices(
        IEnumerable<RouteViewModel> categories,
        IEnumerable<LevelViewModel> levels,
        IEnumerable<ApprenticeshipTypesViewModel> apprenticeshipTypes)
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
            },
            ApprenticeshipTypesChecklistDetails = new ChecklistDetails
            {
                Title = "ApprenticeTypes",
                QueryStringParameterName = "apprenticeshipTypes",
                Lookups = apprenticeshipTypes.OrderBy(x => x.Id).Select(x => new ChecklistLookup(x.Name, x.Value, x.HintText, x.Selected))
            }
        };

    private static string GetPageTitle(SearchResultsViewModel model)
    {
        if (model.Total == 0 || model.NoSearchResultsByUnknownLocation)
            return "No results found";

        return model.Total switch
        {
            1 => $"{model.Total} result found",
            > 1 and <= 10 => $"{model.Total} results found",
            _ =>
                $"{model.Total} results found (page {model.PaginationViewModel.CurrentPage} of {model.PaginationViewModel.TotalPages})"
        };
    }
}