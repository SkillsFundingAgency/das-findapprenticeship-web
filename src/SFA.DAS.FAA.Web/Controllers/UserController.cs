﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UserDateOfBirth;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class UserController(IMediator mediator) : Controller
    {
        [HttpGet]
        [Route("create-account", Name = RouteNames.CreateAccount)]
        public IActionResult CreateAccount()
        {
            return View();
        }
        
        [HttpGet]
        [Route("user-name", Name = RouteNames.UserName)]
        public IActionResult Name()
        {
            return View();
        }

        [HttpPost]
        [Route("user-name", Name = RouteNames.UserName)]
        public async Task<IActionResult> Name(NameViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            try
            {
                var command = new UpdateNameCommand
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    GovIdentifier = User.Claims.GovIdentifier(),
                    Email = User.Claims.Email(),
                };
                await mediator.Send(command);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            return RedirectToRoute(RouteNames.DateOfBirth);
            
        }

        [HttpGet]
        [Route("date-of-birth", Name = RouteNames.DateOfBirth)]
        public IActionResult DateOfBirth() 
        {
            return View();
        }

        [HttpPost]
        [Route("date-of-birth", Name = RouteNames.DateOfBirth)]
        public async Task<IActionResult> DateOfBirth(DateOfBirthViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var command = new UpdateDateOfBirthCommand
                {
                    GovIdentifier = User.Claims.GovIdentifier(),
                    Email = User.Claims.Email(),
                    DateOfBirth = model.DateOfBirth.DateTimeValue.Value
                };
                await mediator.Send(command);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            return RedirectToRoute(RouteNames.SearchResults);

        }
    }
}
