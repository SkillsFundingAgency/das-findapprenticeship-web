using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using MediatR;
using SFA.DAS.FAA.Application.Queries;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.BrowseByInterests;

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
    public async Task<IActionResult> BrowseByInterests([FromQuery] List<int>? routeIds)
    {
        var result = await _mediator.Send(new GetBrowseByInterestsQuery());

        var viewModel = (BrowseByInterestViewModel)result;

        viewModel.AllocateRouteGroup();

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
    public async Task<IActionResult> Location([FromQuery] List<int>? routeIds)
    {
        var viewModel = new LocationViewModel(routeIds);
        return View(viewModel);
    }

}

