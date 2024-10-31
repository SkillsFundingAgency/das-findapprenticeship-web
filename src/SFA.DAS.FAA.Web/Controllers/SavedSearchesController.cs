using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.SavedSearches;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("saved-searches")]
    public class SavedSearchesController(IMediator mediator) : Controller
    {
        [Route("unsubscribe", Name = RouteNames.GetSavedSearchesUnsubscribe)]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]Guid id)
        {
            var result = await mediator.Send(new GetConfirmUnsubscribeQuery
            {
                SavedSearchId = id
            });

            if(result.SavedSearch == null)
            {
                return RedirectToRoute(RouteNames.ServiceStartDefault);
            }

            var viewModel = (UnsubscribeSavedSearchesViewModel?)result;

            return View(viewModel);
        }
    }
}
