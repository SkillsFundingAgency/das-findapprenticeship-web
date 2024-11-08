using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;
using SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch;
using SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Controllers
{
    
    [Route("saved-searches")]
    public class SavedSearchesController(IMediator mediator, IDataProtectorService dataProtectorService, ICacheStorageService cacheStorageService) : Controller
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

            var viewModel = new UnsubscribeSavedSearchesViewModel
            {
                SavedSearch = SavedSearchViewModel.From(result.SavedSearch, result.Routes)
            };

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

            await cacheStorageService.Set($"{model.Id}-savedsearch", model, 5, 5);
            
            return RedirectToRoute(RouteNames.UnsubscribeSavedSearchComplete, new
            {
                model.Id
            });
        }
        
        [HttpGet]
        [Route("unsubscribe-complete", Name = RouteNames.UnsubscribeSavedSearchComplete)]
        public async Task<IActionResult> UnsubscribeComplete(Guid id)
        {
            var model = await cacheStorageService.Get<UnsubscribeSavedSearchesModel?>($"{id}-savedsearch");
            
            if(model == null)
            {
                return RedirectToRoute(RouteNames.ServiceStartDefault);
            }
            
            return View("UnsubscribeComplete", model.Title);
        }
    }
}
