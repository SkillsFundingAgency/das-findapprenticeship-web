using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Controllers
{
    public class UserController(IMediator mediator) : Controller
    {
        [Route("user-name", Name = RouteNames.UserName)]
        public IActionResult Name()
        {
            return View();
        }

        [HttpPost]
        [Route("user-name", Name = RouteNames.UserName)]
        public async Task<IActionResult> Name(NameViewModel model) 
        {
            var command = new UpdateNameCommand
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            try
            {
                await mediator.Send(command);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            //TODO add correct route 
            return RedirectToRoute(RouteNames.SearchResults);
            
        }
    }
}
