using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using MediatR;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

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
            var result = await _mediator.Send(new GetBrowseByInterestsQuery());

            var viewModel = (BrowseByInterestViewModel)result;
            viewModel.ErrorDictionary = ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                    );

            viewModel.AllocateRouteGroup();

            return View(viewModel);
        }

        return RedirectToRoute(RouteNames.Location, new { RouteIds = model.SelectedRouteIds });
    }

    [Route("location", Name = RouteNames.Location)]
    public async Task<IActionResult> Location([FromQuery] List<string>? routeIds)
    {
        var viewModel = new LocationViewModel(routeIds);
        return View(viewModel);
    }

    [HttpPost]
    [Route("location", Name = RouteNames.Location)]
    public async Task<IActionResult> Location([FromQuery] List<string>? routeIds, LocationViewModel model)
    {
        if (model.cityOrPostcodeSelected == null && model.allOfEngland == null)
        {
            ModelState.AddModelError(string.Empty, "Select if you want to enter a city or postcode or if you want to search across all of England");
        }
        else if ((bool)model.cityOrPostcodeSelected && model.cityOrPostcode == null)
        {
            ModelState.AddModelError(string.Empty, "Enter a city or postcode");
        }

        if (!ModelState.IsValid)
        {
            model.ErrorDictionary = ModelState
                .Where(x => x.Value is { Errors.Count: > 0 })
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );

            return View(model);
        }

        return View(model);
    }

}

