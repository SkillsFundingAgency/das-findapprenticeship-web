using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Controllers;

[Route("[controller]")]
public class LocationsController : Controller
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> LocationsBySearch([FromQuery] string searchTerm)
    {
        var result = await _mediator.Send(new GetLocationsBySearchQuery
        {
            SearchTerm = searchTerm
        });

        var model = new LocationViewModel
        {
            Locations = new LocationsBySearchViewModel { Locations = result.LocationItems.Select(c => (LocationBySearchViewModel)c).ToList() }
        };

        return new JsonResult(model);
    }
}
