using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Controllers;

[Route("[controller]")]
public class LocationsController(IMediator mediator) : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> LocationsBySearch([FromQuery] string searchTerm)
    {
        var result = await mediator.Send(new GetLocationsBySearchQuery
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
