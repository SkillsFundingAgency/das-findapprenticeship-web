using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.SavedSearches;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;

namespace SFA.DAS.FAA.Web.Controllers
{
    
    [Route("saved-searches")]
    public class SavedSearchesController(IMediator mediator, IDataProtectorService dataProtectorService) : Controller
    {
        [HttpGet]
        [Route("unsubscribe", Name = RouteNames.GetSavedSearchesUnsubscribe)]
        public async Task<IActionResult> Index([FromQuery]string id)
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

        [HttpPost]
        [Route("unsubscribe", Name = RouteNames.PostSavedSearchesUnsubscribe)]
        public async Task<IActionResult> Index(UnsubscribeSavedSearchesModel model)
        {
            throw new NotImplementedException();
        }
        
        [HttpGet]
        [Route("unsubscribe-complete", Name = RouteNames.PostSavedSearchesUnsubscribe)]
        public async Task<IActionResult> UnsubscribeComplete()
        {
            throw new NotImplementedException();
        }
    }
}
