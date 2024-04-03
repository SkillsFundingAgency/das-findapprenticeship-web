using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CandidatePreferences;
using SFA.DAS.FAA.Application.Commands.ManuallyEnteredAddress;
using SFA.DAS.FAA.Application.Commands.PhoneNumber;
using SFA.DAS.FAA.Application.Commands.UserAddress;
using SFA.DAS.FAA.Application.Commands.UserDateOfBirth;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
using SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
using SFA.DAS.FAA.Application.Queries.User.GetCandidateDateOfBirth;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcodeAddress;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Models.User;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

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
        public IActionResult Name(bool? change = false)
        {
            var model = new NameViewModel
            {
                ReturnToConfirmationPage = change,
                BackLink = change is true ? RouteNames.ConfirmAccountDetails : RouteNames.CreateAccount
            };
            return View(model);
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

            return model.ReturnToConfirmationPage is true ?
                RedirectToRoute(RouteNames.ConfirmAccountDetails)
                : RedirectToRoute(RouteNames.DateOfBirth);

        }

        [HttpGet]
        [Route("date-of-birth", Name = RouteNames.DateOfBirth)]
        public async Task<IActionResult> DateOfBirth(bool? change = false)
        {
            var result = await mediator.Send(new GetCandidateDateOfBirthQuery
            {
                GovUkIdentifier = User.Claims.GovIdentifier()
            });

            var model = new DateOfBirthViewModel
            {
                DateOfBirth = result.DateOfBirth != null ? new DayMonthYearDate(result.DateOfBirth) : null,
                ReturnToConfirmationPage = change,
                BackLink = change is true ? RouteNames.ConfirmAccountDetails : RouteNames.UserName
            };
            return View(model);
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

            return model.ReturnToConfirmationPage is true ?
                RedirectToRoute(RouteNames.ConfirmAccountDetails)
                : RedirectToRoute(RouteNames.PostcodeAddress);

        }

        [HttpGet("postcode-address", Name = RouteNames.PostcodeAddress)]
        public IActionResult PostcodeAddress(string? postcode, bool? change = false)
        {
            var model = new PostcodeAddressViewModel()
            {
                Postcode = postcode,
                ReturnToConfirmationPage = change,
                BackLink = change is true ? RouteNames.ConfirmAccountDetails : RouteNames.DateOfBirth
            };
            return View(model);
        }

        [HttpPost("postcode-address", Name = RouteNames.PostcodeAddress)]
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

            return RedirectToRoute
                (RouteNames.SelectAddress, new { postcode = model.Postcode, change = model.ReturnToConfirmationPage });
        }

        [HttpGet("select-address", Name = RouteNames.SelectAddress)]
        public async Task<IActionResult> SelectAddress(string? postcode, bool? change = false)
        {
            var userPostcode = postcode;
            if (userPostcode == null)
            {
                var queryResult = await mediator.Send(new GetCandidateAddressQuery()
                {
                    CandidateId = User.Claims.CandidateId()
                });

                userPostcode = queryResult.Postcode;
            }
            var result = await mediator.Send(new GetAddressesByPostcodeQuery() { Postcode = userPostcode });

            var model = (SelectAddressViewModel)result.Addresses?.ToList();
            model.Postcode = model.Addresses?.FirstOrDefault()?.Postcode ?? userPostcode;
            model.ReturnToConfirmationPage = change;

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
            return model.ReturnToConfirmationPage is true ?
                RedirectToRoute(RouteNames.ConfirmAccountDetails)
                : RedirectToRoute(RouteNames.PhoneNumber, new { backLink = RouteNames.SelectAddress.ToString() });
        }

        [HttpGet("enter-address", Name = RouteNames.EnterAddressManually)]
        public async Task<IActionResult> EnterAddressManually(string? backLink, string? selectAddressPostcode, bool? change = false)
        {
            var model = new EnterAddressManuallyViewModel()
            {
                BackLink = backLink ?? RouteNames.PostcodeAddress,
                SelectAddressPostcode = selectAddressPostcode,
                ReturnToConfirmationPage = change
            };

            var result = await mediator.Send(new GetCandidateAddressQuery()
            {
                CandidateId = User.Claims.CandidateId()
            });

            if (result.AddressLine1 != null)
            {
                model.AddressLine1 = result.AddressLine1;
                model.AddressLine2 = result.AddressLine2 ?? null;
                model.TownOrCity = result.Town;
                model.County = result.County ?? null;
                model.Postcode = result.Postcode ?? null;
            }

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

            return model.ReturnToConfirmationPage is true ?
                RedirectToRoute(RouteNames.ConfirmAccountDetails)
                : RedirectToRoute(RouteNames.PhoneNumber, new { backLink = RouteNames.EnterAddressManually });
        }

        [HttpGet("phone-number", Name = RouteNames.PhoneNumber)]
        public IActionResult PhoneNumber(string? backLink, bool? change = false)
        {
            var model = new PhoneNumberViewModel()
            {
                PhoneNumber = User.Claims.PhoneNumber() != null ? User.Claims.PhoneNumber() : null,
                Backlink = backLink,
                ReturnToConfirmationPage = change
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


            return model.ReturnToConfirmationPage is true ?
               RedirectToRoute(RouteNames.ConfirmAccountDetails)
               : RedirectToRoute(RouteNames.NotificationPreferences, new { phoneNumberBackLink = model.Backlink });
        }

        [HttpGet("notification-preferences", Name = RouteNames.NotificationPreferences)]
        public async Task<IActionResult> NotificationPreferences(string? phoneNumberBackLink, bool? change = false)
        {
            var candidatePreferences = await mediator.Send(new GetCandidatePreferencesQuery
            {
                CandidateId = User.Claims.CandidateId()
            });

            var model = new NotificationPreferencesViewModel()
            {
                NotificationPreferences = candidatePreferences.CandidatePreferences.Select(cp => new NotificationPreferenceItemViewModel
                {
                    PreferenceId = cp.PreferenceId,
                    Meaning = cp.PreferenceMeaning,
                    Hint = cp.PreferenceHint,
                    EmailPreference = cp.ContactMethodsAndStatus?.Where(x => x.ContactMethod == CandidatePreferencesConstants.ContactMethodEmail).FirstOrDefault()?.Status ?? false,
                    TextPreference = cp.ContactMethodsAndStatus?.Where(x => x.ContactMethod == CandidatePreferencesConstants.ContactMethodText).FirstOrDefault()?.Status ?? false
                }).ToList(),
                PhoneNumberBacklink = phoneNumberBackLink,
                ReturnToConfirmationPage = change
            };

            return View(model);
        }

        [HttpPost("notification-preferences", Name = RouteNames.NotificationPreferences)]
        public async Task<IActionResult> NotificationPreferences(NotificationPreferencesViewModel model)
        {
            await mediator.Send(new UpsertCandidatePreferencesCommand
            {
                CandidateEmail = User.Claims.Email(),
                CandidateId = User.Claims.CandidateId(),
                NotificationPreferences = model.NotificationPreferences.Select(x => new NotificationPreferenceItem
                {
                    PreferenceId = x.PreferenceId,
                    Meaning = x.Meaning,
                    Hint = x.Hint,
                    EmailPreference = x.EmailPreference,
                    TextPreference = x.TextPreference
                }).ToList()
            });

            return model.ReturnToConfirmationPage is true ?
               RedirectToRoute(RouteNames.ConfirmAccountDetails)
               : RedirectToRoute(RouteNames.ConfirmAccountDetails);
        }

        [HttpGet("notification-preferences-skip", Name = RouteNames.NotificationPreferencesSkip)]
        public async Task<IActionResult> SkipNotificationPreferences()
        {
            var candidatePreferences = await mediator.Send(new GetCandidatePreferencesQuery
            {
                CandidateId = User.Claims.CandidateId()
            });

            var model = new NotificationPreferencesViewModel()
            {
                NotificationPreferences = candidatePreferences.CandidatePreferences.Select(cp => new NotificationPreferenceItemViewModel
                {
                    PreferenceId = cp.PreferenceId,
                    Meaning = cp.PreferenceMeaning,
                    Hint = cp.PreferenceHint,
                    EmailPreference = false,
                    TextPreference = false
                }).ToList()
            };

            return await NotificationPreferences(model);
        }

        [HttpGet("check-your-account-details", Name = RouteNames.ConfirmAccountDetails)]
        public async Task<IActionResult> ConfirmYourAccountDetails()
        {
            var accountDetails = await mediator.Send(new GetCandidateAccountDetailsQuery
            {
                CandidateId = User.Claims.CandidateId(),
                GovUkIdentifier = User.Claims.GovIdentifier()
            });

            var model = new ConfirmAccountDetailsViewModel
            {
                FirstName = accountDetails.FirstName,
                MiddleNames = accountDetails.MiddleNames,
                LastName = accountDetails.LastName,
                PhoneNumber = User.Claims.PhoneNumber(),
                DateOfBirth = accountDetails.DateOfBirth,
                EmailAddress = User.Claims.Email(),
                AddressLine1 = accountDetails.AddressLine1,
                AddressLine2 = accountDetails.AddressLine2,
                County = accountDetails.County,
                Town = accountDetails.Town,
                Postcode = accountDetails.Postcode,
                CandidatePreferences = accountDetails.CandidatePreferences.Select(cp => new ConfirmAccountDetailsViewModel.CandidatePreference
                {
                    PreferenceId = cp.PreferenceId,
                    Meaning = cp.Meaning,
                    Hint = cp.Hint,
                    EmailPreference = cp.EmailPreference,
                    TextPreference = cp.TextPreference
                }).ToList()
            };

            return View(model);
        }
    }
}
