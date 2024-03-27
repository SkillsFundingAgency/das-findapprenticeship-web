using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.ManuallyEnteredAddress;
using SFA.DAS.FAA.Application.Commands.PhoneNumber;
using SFA.DAS.FAA.Application.Commands.UserAddress;
using SFA.DAS.FAA.Application.Commands.UserDateOfBirth;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcodeAddress;
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
            catch
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
            catch
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            return RedirectToRoute(RouteNames.PostcodeAddress);

        }

        [HttpGet("address", Name = RouteNames.PostcodeAddress)]
        public IActionResult PostcodeAddress()
        {
            return View();
        }

        [HttpPost("address", Name = RouteNames.PostcodeAddress)]
        public async Task<IActionResult> PostcodeAddress(PostcodeAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var postcodeExists = await mediator.Send(new GetCandidatePostcodeAddressQuery() { Postcode = model.Postcode! });

                if (!postcodeExists.PostcodeExists)
                {
                    ModelState.AddModelError(nameof(PostcodeAddressViewModel.Postcode), "Enter a recognised postcode or select 'Enter my address manually'");
                    return View(model);
                }
            }
            catch
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            return RedirectToRoute(RouteNames.SelectAddress, new { model.Postcode });
        }

        [HttpGet("select-address", Name = RouteNames.SelectAddress)]
        public async Task<IActionResult> SelectAddress(string postcode)
        {
            var result = await mediator.Send(new GetAddressesByPostcodeQuery() { Postcode = postcode });

            var model = (SelectAddressViewModel)result.Addresses?.ToList();
            model.Postcode = model.Addresses?.FirstOrDefault()?.Postcode ?? postcode;

            return View(model);
        }

        [HttpPost("select-address", Name = RouteNames.SelectAddress)]
        public async Task<IActionResult> SelectAddress(SelectAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetAddressesByPostcodeQuery() { Postcode = model.Postcode });
                var addressesModel = (SelectAddressViewModel)result.Addresses?.ToList();
                model.Addresses = addressesModel.Addresses;
                return View(model);
            }

            var addresses = await mediator.Send(new GetAddressesByPostcodeQuery() { Postcode = model.Postcode });
            model.Addresses = addresses.Addresses?.Select(x => (AddressViewModel)x).ToList();

            var selectedAdress = addresses.Addresses?.Where(x => x.Uprn == model.SelectedAddress).SingleOrDefault();
            await mediator.Send(new UpdateAddressCommand()
            {
                CandidateId = User.Claims.CandidateId(),
                Email = User.Claims.Email(),
                Thoroughfare = selectedAdress.Thoroughfare,
                Organisation = selectedAdress.Organisation,
                AddressLine1 = selectedAdress.AddressLine1,
                AddressLine2 = selectedAdress.AddressLine2,
                AddressLine3 = selectedAdress.PostTown,
                AddressLine4 = selectedAdress.County,
                Postcode = selectedAdress.Postcode
            });

            return RedirectToRoute(RouteNames.PhoneNumber, new {backLink = RouteNames.SelectAddress.ToString()});
        }

        [HttpGet("enter-address", Name = RouteNames.EnterAddressManually)]
        public IActionResult EnterAddressManually(string backLink, string? selectAddressPostcode)
        {
            var model = new EnterAddressManuallyViewModel() { BackLink = backLink, SelectAddressPostcode = selectAddressPostcode };
            return View(model);
        }

        [HttpPost("enter-address", Name = RouteNames.EnterAddressManually)]
        public async Task<IActionResult> EnterAddressManually(EnterAddressManuallyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.Send(new UpdateManuallyEnteredAddressCommand()
            {
                CandidateId = User.Claims.CandidateId(),
                Email = User.Claims.Email(),
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                TownOrCity = model.TownOrCity,
                County = model.County,
                Postcode = model.Postcode
            });

            return RedirectToRoute(RouteNames.PhoneNumber, new {RouteNames.EnterAddressManually});
        }

        [HttpGet("phone-number", Name = RouteNames.PhoneNumber)]
        public IActionResult PhoneNumber(string backLink)
        {
            var model = new PhoneNumberViewModel()
            {
                PhoneNumber = User.Claims.PhoneNumber() != null ? User.Claims.PhoneNumber() : null,
                Backlink = backLink
            };

            return View(model);
        }

        [HttpPost("phone-number", Name = RouteNames.PhoneNumber)]
        public async Task<IActionResult> PhoneNumber(PhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.Send(new UpdatePhoneNumberCommand()
            {
                GovUkIdentifier = User.Claims.GovIdentifier(),
                Email = User.Claims.Email(),
                PhoneNumber = model.PhoneNumber
            });


            return RedirectToRoute(RouteNames.NotificationPreferences);
        }
    }
}
