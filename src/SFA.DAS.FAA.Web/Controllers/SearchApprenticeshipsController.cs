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
    public async Task<IActionResult> BrowseByInterests()
    {
        var result = await _mediator.Send(new GetBrowseByInterestsQuery());

        var viewModel = (BrowseByInterestViewModel)result;

        viewModel.allocateRouteGroup();

        return View(viewModel);
    }

    [HttpPost]
    [Route("browse-by-interests", Name = RouteNames.BrowseByInterests)]
    public async Task<IActionResult> BrowseByInterests(BrowseByInterestViewModel model)

    {
        if (!model.SelectedRouteIds.Any())
        {
            var viewModel = new BrowseByInterestViewModel()
            {
                ErrorDictionary = ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                    ),
                Routes = model.Routes,

                //agricultureEnvironmentalAndAnimalCareDictionary = model.agricultureEnvironmentalAndAnimalCareDictionary,
                //businessSalesAndLegalDictionary = model.businessSalesAndLegalDictionary,
                //careHealthAndScienceDictionary = model.careHealthAndScienceDictionary,
                //cateringAndHospitalityDictionary = model.cateringAndHospitalityDictionary,
                //constructionEngineeringAndBuildingsDictionary = model.constructionEngineeringAndBuildingsDictionary,
                //creativeAndDesignDictionary = model.creativeAndDesignDictionary,
                //digitalDictionary = model.digitalDictionary,
                //educationAndEarlyYearsDictionary = model.educationAndEarlyYearsDictionary,
                //protectiveServicesDictionary = model.protectiveServicesDictionary,
                //transportAndLogisticsDictionary = model.transportAndLogisticsDictionary,

            };
            viewModel.allocateRouteGroup();
            return View(viewModel);
        }

        return View(model);
    }
}

