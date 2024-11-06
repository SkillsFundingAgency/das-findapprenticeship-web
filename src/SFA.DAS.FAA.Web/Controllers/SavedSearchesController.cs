using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.SavedSearches;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;
using SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch;

namespace SFA.DAS.FAA.Web.Controllers
{
    
    [Route("saved-searches")]
    public class SavedSearchesController(IMediator mediator, IDataProtectorService dataProtectorService) : Controller
    {
        [HttpGet]
        [Route("unsubscribe", Name = RouteNames.GetSavedSearchesUnsubscribe)]
        public async Task<IActionResult> Index([FromQuery]string id)
        {            
            var savedSearchId = dataProtectorService.DecodeData(id);
            if (!Guid.TryParse(savedSearchId, out var searchId)) 
            {
                return RedirectToRoute(RouteNames.ServiceStartDefault);
            }
            
            var result = await mediator.Send(new GetConfirmUnsubscribeQuery
            {
                SavedSearchId = searchId
            });

            if(result.SavedSearch == null)
            {
                return RedirectToRoute(RouteNames.ServiceStartDefault);
            }

            var viewModel = (UnsubscribeSavedSearchesViewModel?)result;

            return View(viewModel);
        }

        [HttpPost]
        [Route("unsubscribe", Name = RouteNames.PostSavedSearchesUnsubscribe)]
        public async Task<IActionResult> Index(UnsubscribeSavedSearchesModel model)
        {
            await mediator.Send(new UnsubscribeSavedSearchCommand
            {
                SavedSearchId = model.Id
            });

            return RedirectToRoute(RouteNames.UnsubscribeSavedSearchComplete, new
            {
                // Need null coalescing operator here?
                SearchTitle = model.SearchTitle
            });
        }
        
        [HttpGet]
        [Route("unsubscribe-complete", Name = RouteNames.UnsubscribeSavedSearchComplete)]
        public ActionResult UnsubscribeComplete(string searchTitle)
        {

            return View("UnsubscribeComplete", searchTitle);
        }
    }
}
