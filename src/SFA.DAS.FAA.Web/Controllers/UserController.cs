using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Runtime.InteropServices;

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
        public async Task<IActionResult> Name([FromQuery] string firstName, [FromQuery] string lastName) 
        {
            var command = new UpdateNameCommand
            {
                FirstName = firstName,
                LastName = lastName
            };

            await mediator.Send(command);

            //TODO add correct route 
            return RedirectToRoute("/"); //check if it has been sucessful? 
            
        }
    }
}
