using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CheckAnswers;
using SFA.DAS.FAA.Application.Commands.CreateAccount.ManuallyEnteredAddress;
using SFA.DAS.FAA.Application.Commands.CreateAccount.PhoneNumber;
using SFA.DAS.FAA.Application.Commands.CreateAccount.SelectedAddress;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserDateOfBirth;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserName;
using CreateAccount.GetAddressesByPostcode;
using CreateAccount.GetCandidateAccountDetails;
using CreateAccount.GetCandidateDateOfBirth;
using CreateAccount.GetCandidateName;
using CreateAccount.GetCandidatePhoneNumber;
using CreateAccount.GetCandidatePostcode;
using CreateAccount.GetCandidatePostcodeAddress;
using CreateAccount.GetCandidatePreferences;
using SFA.DAS.FAA.Application.Commands.Applications.LegacyApplications;
using SFA.DAS.FAA.Application.Queries.Applications.GetTransferUserData;
using SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform;
using SFA.DAS.FAA.Application.Queries.User.GetSignIntoYourOldAccount;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Models.User;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("create-account")]
    public class UserController(IMediator mediator, ICacheStorageService cacheStorageService) : Controller
    {
        [HttpGet]
        [Route("", Name = RouteNames.CreateAccount)]
        public async Task<IActionResult> CreateAccount([FromQuery] string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                cacheStorageService.Set($"{User.Claims.GovIdentifier()}-{CacheKeys.CreateAccountReturnUrl}", returnUrl);
            }

            var result = await mediator.Send(new GetInformQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new InformViewModel
            {
                ShowAccountRecoveryBanner = result.ShowAccountRecoveryBanner
            };

            return View(model);
        }

        [HttpGet]
        [Route("transfer-your-data", Name = RouteNames.TransferYourData)]
        public IActionResult TransferYourData()
        {
            return View();
        }

        [HttpGet]
        [Route("sign-in-to-your-old-account", Name = RouteNames.SignInToYourOldAccount)]
        public IActionResult SignInToYourOldAccount()
        {
            var viewModel = new SignInToYourOldAccountViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("sign-in-to-your-old-account", Name = RouteNames.SignInToYourOldAccount)]
        public async Task<IActionResult> SignInToYourOldAccount(SignInToYourOldAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await mediator.Send(new GetSignIntoYourOldAccountQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Email = viewModel.Email ?? "",
                Password = viewModel.Password ?? ""
            });

            if (!result.IsValid)
            {
                ModelState.AddModelError(nameof(SignInToYourOldAccountViewModel.Password), "Check your account details. You’ve entered an incorrect email address or password.");
                return View(viewModel);
            }

            await cacheStorageService.Set($"{User.Claims.CandidateId()}-{CacheKeys.LegacyEmail}", viewModel.Email);

            //todo: replace with redirect to preview page
            return Ok("Login successful");
        }

        [HttpGet]
        [Route("user-name", Name = RouteNames.UserName)]
        public async Task<IActionResult> Name(bool? change = false)
        {
            var name = await mediator.Send(new GetCandidateNameQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new NameViewModel
            {
                FirstName = name?.FirstName ?? null,
                LastName = name?.LastName ?? null,
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
                    CandidateId = (Guid)User.Claims.CandidateId()!,
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
                CandidateId = (Guid)User.Claims.CandidateId()!
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
                    CandidateId = (Guid)User.Claims.CandidateId()!,
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
        public IActionResult PostcodeAddress(string? postcode, bool change = false)
        {
            var model = new PostcodeAddressViewModel()
            {
                Postcode = postcode,
                ReturnToConfirmationPage = change,
                IsEdit = change
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
        public async Task<IActionResult> SelectAddress(string? postcode, bool change = false)
        {
            var result = await mediator.Send(new GetAddressesByPostcodeQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Postcode = postcode
            });

            var model = (SelectAddressViewModel)result;
            model.ReturnToConfirmationPage = change;
            model.IsEdit = change;

            return View(model);
        }

        [HttpPost("select-address", Name = RouteNames.SelectAddress)]
        public async Task<IActionResult> SelectAddress(SelectAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetAddressesByPostcodeQuery() { Postcode = model.Postcode });
                var addressesModel = (SelectAddressViewModel) result;
                return View(addressesModel);
            }

            var addresses = await mediator.Send(new GetAddressesByPostcodeQuery() { Postcode = model.Postcode });
            model.Addresses = addresses.Addresses?.Select(x => (AddressViewModel)x).ToList();

            var selectedAddress = addresses.Addresses?.Where(x => x.Uprn == model.SelectedAddress).SingleOrDefault();
            await mediator.Send(new UpdateAddressCommand()
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Email = User.Claims.Email(),
                Uprn = selectedAddress.Uprn,
                Thoroughfare = selectedAddress.Thoroughfare,
                Organisation = selectedAddress.Organisation,
                AddressLine1 = selectedAddress.AddressLine1,
                AddressLine2 = selectedAddress.AddressLine2,
                AddressLine3 = selectedAddress.PostTown,
                AddressLine4 = selectedAddress.County,
                Postcode = selectedAddress.Postcode
            });
            return model.ReturnToConfirmationPage is true ?
                RedirectToRoute(RouteNames.ConfirmAccountDetails)
                : RedirectToRoute(RouteNames.PhoneNumber, new { backLink = RouteNames.SelectAddress });
        }

        [HttpGet("enter-address", Name = RouteNames.EnterAddressManually)]
        public async Task<IActionResult> EnterAddressManually(string? backLink, string? selectAddressPostcode, bool change = false)
        {
            var result = await mediator.Send(new GetCandidateAddressQuery()
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new EnterAddressManuallyViewModel
            {
                SelectAddressPostcode = selectAddressPostcode,
                IsEdit = change
            };

            if (change)
            {
                model.BackLink = !result.IsAddressFromLookup ? RouteNames.ConfirmAccountDetails : backLink ?? RouteNames.PostcodeAddress;
            }
            else
            {
                model.BackLink = result.IsAddressFromLookup ? RouteNames.SelectAddress : RouteNames.PostcodeAddress;
            }

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
                CandidateId = (Guid)User.Claims.CandidateId()!,
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
        public async Task<IActionResult> PhoneNumber(string? backLink, bool change=false)
        {
            var queryResult = await mediator.Send(new GetCandidatePhoneNumberQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new PhoneNumberViewModel
            {
                PhoneNumber = queryResult.PhoneNumber,
                ReturnToConfirmationPage = change,
                BackLink = change ? RouteNames.ConfirmAccountDetails 
                    : queryResult.IsAddressFromLookup ? RouteNames.SelectAddress : RouteNames.EnterAddressManually,
                Postcode = queryResult.Postcode
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
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Email = User.Claims.Email(),
                PhoneNumber = model.PhoneNumber
            });


            return model.ReturnToConfirmationPage is true ?
               RedirectToRoute(RouteNames.ConfirmAccountDetails)
               : RedirectToRoute(RouteNames.NotificationPreferences, new { phoneNumberBackLink = model.BackLink });
        }

        [HttpGet("notification-preferences", Name = RouteNames.NotificationPreferences)]
        public async Task<IActionResult> NotificationPreferences(string? phoneNumberBackLink, bool? change = false)
        {
            var candidatePreferences = await mediator.Send(new GetCandidatePreferencesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
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
                }).OrderByDescending(c=>c.Meaning).ToList(),
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
                CandidateId = (Guid)User.Claims.CandidateId()!,
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
                CandidateId = (Guid)User.Claims.CandidateId()!
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

        [HttpGet("check-answers", Name = RouteNames.ConfirmAccountDetails)]
        public async Task<IActionResult> CheckAnswers()
        {
            var accountDetails = await mediator.Send(new GetCandidateAccountDetailsQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new ConfirmAccountDetailsViewModel
            {
                FirstName = accountDetails.FirstName,
                MiddleNames = accountDetails.MiddleNames,
                LastName = accountDetails.LastName,
                PhoneNumber = accountDetails.PhoneNumber,
                DateOfBirth = accountDetails.DateOfBirth,
                IsAddressFromLookup = accountDetails.Uprn != null,
                EmailAddress = accountDetails.Email,
                AddressLine1 = accountDetails.AddressLine1,
                AddressLine2 = accountDetails.AddressLine2,
                County = accountDetails.County,
                Town = accountDetails.Town,
                Postcode = accountDetails.Postcode,
                Uprn = accountDetails.Uprn,
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

        [HttpPost("check-answers", Name = RouteNames.ConfirmAccountDetails)]
        public async Task<IActionResult> ConfirmYourAccountDetails(ConfirmAccountDetailsViewModel model)
        {
            await mediator.Send(new UpdateCheckAnswersCommand
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });


            var returnUrl = await cacheStorageService.Get<string>($"{User.Claims.GovIdentifier()}-{CacheKeys.CreateAccountReturnUrl}");

            await cacheStorageService.Set($"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}", true);

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToRoute(RouteNames.ServiceStartDefault);
        }

        [HttpGet("confirm-transfer", Name = RouteNames.ConfirmTransferYourData)]
        public async Task<IActionResult> ConfirmDataTransfer()
        {
            var response = await mediator.Send(new GetTransferUserDataQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                EmailAddress = "Email from cache"
            });

            var model = (ConfirmTransferViewModel) response;
            model.EmailAddress = User.Claims.Email()!;
            
            return View(model);
        }

        [HttpPost("confirm-transfer", Name = RouteNames.ConfirmTransferYourData)]
        public async Task<IActionResult> ConfirmDataTransfer(ConfirmTransferViewModel viewModel)
        {
            await mediator.Send(new MigrateLegacyApplicationsCommand
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                EmailAddress = "Email from cache"
            });

            return RedirectToRoute(RouteNames.FinishAccountSetup);
        }
    }
}