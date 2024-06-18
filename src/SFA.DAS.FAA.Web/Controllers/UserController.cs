using CreateAccount.GetAddressesByPostcode;
using CreateAccount.GetCandidateAccountDetails;
using CreateAccount.GetCandidateDateOfBirth;
using CreateAccount.GetCandidateName;
using CreateAccount.GetCandidatePhoneNumber;
using CreateAccount.GetCandidatePostcode;
using CreateAccount.GetCandidatePostcodeAddress;
using CreateAccount.GetCandidatePreferences;
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
using SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform;
using SFA.DAS.FAA.Application.Queries.User.GetSettings;
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
    [Route("user")]
    public class UserController(IMediator mediator, ICacheStorageService cacheStorageService) : Controller
    {
        [HttpGet]
        [Route("create-account", Name = RouteNames.CreateAccount)]
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
        [Route("create-account/transfer-your-data", Name = RouteNames.TransferYourData)]
        public IActionResult TransferYourData()
        {
            return View();
        }

        [HttpGet]
        [Route("create-account/sign-in-to-your-old-account", Name = RouteNames.SignInToYourOldAccount)]
        public IActionResult SignInToYourOldAccount()
        {
            var viewModel = new SignInToYourOldAccountViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("create-account/sign-in-to-your-old-account", Name = RouteNames.SignInToYourOldAccount)]
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
        public async Task<IActionResult> Name(UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var name = await mediator.Send(new GetCandidateNameQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new NameViewModel
            {
                FirstName = name.FirstName,
                LastName = name.LastName,
                JourneyPath = journeyPath,
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
                    Email = User.Claims.Email()!,
                };
                await mediator.Send(command);
            }
            catch
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            return RedirectToRoute(model.RedirectRoute);
        }

        [HttpGet]
        [Route("date-of-birth", Name = RouteNames.DateOfBirth)]
        public async Task<IActionResult> DateOfBirth(UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var result = await mediator.Send(new GetCandidateDateOfBirthQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new DateOfBirthViewModel
            {
                DateOfBirth = result.DateOfBirth != null ? new DayMonthYearDate(result.DateOfBirth) : null,
                JourneyPath = journeyPath
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
                    Email = User.Claims.Email()!,
                    DateOfBirth = model.DateOfBirth!.DateTimeValue!.Value
                };
                await mediator.Send(command);
            }
            catch
            {
                ModelState.AddModelError(nameof(NameViewModel), "There's been a problem");
                return View(model);
            }

            return RedirectToRoute(model.RedirectRoute);
        }

        [HttpGet("postcode-address", Name = RouteNames.PostcodeAddress)]
        public IActionResult PostcodeAddress(string? postcode, UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var model = new PostcodeAddressViewModel
            {
                Postcode = postcode,
                JourneyPath = journeyPath
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
                var postcodeExists = await mediator.Send(new GetCandidatePostcodeAddressQuery { Postcode = model.Postcode! });

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
                (RouteNames.SelectAddress, new { postcode = model.Postcode, journeyPath = model.JourneyPath });
        }

        [HttpGet("select-address", Name = RouteNames.SelectAddress)]
        public async Task<IActionResult> SelectAddress(string? postcode, UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var result = await mediator.Send(new GetAddressesByPostcodeQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Postcode = postcode
            });

            var model = (SelectAddressViewModel)result;
            model.JourneyPath = journeyPath;
            return View(model);
        }

        [HttpPost("select-address", Name = RouteNames.SelectAddress)]
        public async Task<IActionResult> SelectAddress(SelectAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetAddressesByPostcodeQuery { Postcode = model.Postcode });
                var addressesModel = (SelectAddressViewModel) result;
                return View(addressesModel);
            }

            var addresses = await mediator.Send(new GetAddressesByPostcodeQuery { Postcode = model.Postcode });
            model.Addresses = addresses.Addresses?.Select(x => (AddressViewModel)x).ToList();

            var selectedAddress = addresses.Addresses?.Where(x => x.Uprn == model.SelectedAddress).SingleOrDefault();
            await mediator.Send(new UpdateAddressCommand
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Email = User.Claims.Email()!,
                Uprn = selectedAddress.Uprn,
                Thoroughfare = selectedAddress.Thoroughfare,
                Organisation = selectedAddress.Organisation,
                AddressLine1 = selectedAddress.AddressLine1,
                AddressLine2 = selectedAddress.AddressLine2,
                AddressLine3 = selectedAddress.PostTown,
                AddressLine4 = selectedAddress.County,
                Postcode = selectedAddress.Postcode
            });

            return model.RedirectRoute == RouteNames.PhoneNumber
                ? RedirectToRoute(RouteNames.PhoneNumber, new { journeyPath = model.JourneyPath })
                : RedirectToRoute(model.RedirectRoute);
        }

        [HttpGet("enter-address", Name = RouteNames.EnterAddressManually)]
        public async Task<IActionResult> EnterAddressManually(string? selectAddressPostcode, UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var result = await mediator.Send(new GetCandidateAddressQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new EnterAddressManuallyViewModel
            {
                SelectAddressPostcode = selectAddressPostcode,
                JourneyPath = journeyPath,
            };

            if (journeyPath != UserJourneyPath.CreateAccount)
            {
                model.BackLink = model.CustomBackLink;
            }
            else
            {
                model.BackLink = result.IsAddressFromLookup ? RouteNames.SelectAddress : RouteNames.PostcodeAddress;
            }

            model.AddressLine1 = result.AddressLine1;
            model.AddressLine2 = result.AddressLine2 ?? null;
            model.TownOrCity = result.Town;
            model.County = result.County ?? null;
            model.Postcode = result.Postcode ?? null;

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
                Email = User.Claims.Email()!,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                TownOrCity = model.TownOrCity,
                County = model.County,
                Postcode = model.Postcode
            });

            return model.RedirectRoute == RouteNames.PhoneNumber
                ? RedirectToRoute(RouteNames.PhoneNumber, new { journeyPath = UserJourneyPath.EnterAddressManually })
                : RedirectToRoute(model.RedirectRoute);
        }

        [HttpGet("phone-number", Name = RouteNames.PhoneNumber)]
        public async Task<IActionResult> PhoneNumber(UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var queryResult = await mediator.Send(new GetCandidatePhoneNumberQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new PhoneNumberViewModel
            {
                PhoneNumber = queryResult.PhoneNumber,
                JourneyPath = journeyPath,
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

            await mediator.Send(new UpdatePhoneNumberCommand
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Email = User.Claims.Email()!,
                PhoneNumber = model.PhoneNumber!
            });

            return model.RedirectRoute == RouteNames.NotificationPreferences 
                ? RedirectToRoute(RouteNames.NotificationPreferences, new { journeyPath = model.JourneyPath }) 
                : RedirectToRoute(model.RedirectRoute);
        }

        [HttpGet("notification-preferences", Name = RouteNames.NotificationPreferences)]
        public async Task<IActionResult> NotificationPreferences(UserJourneyPath journeyPath = UserJourneyPath.CreateAccount, bool? change = false)
        {
            var candidatePreferences = await mediator.Send(new GetCandidatePreferencesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new NotificationPreferencesViewModel
            {
                NotificationPreferences = candidatePreferences.CandidatePreferences.Select(cp => new NotificationPreferenceItemViewModel
                {
                    PreferenceId = cp.PreferenceId,
                    Meaning = cp.PreferenceMeaning,
                    Hint = cp.PreferenceHint,
                    EmailPreference = cp.ContactMethodsAndStatus?.Where(x => x.ContactMethod == CandidatePreferencesConstants.ContactMethodEmail).FirstOrDefault()?.Status ?? false,
                    TextPreference = cp.ContactMethodsAndStatus?.Where(x => x.ContactMethod == CandidatePreferencesConstants.ContactMethodText).FirstOrDefault()?.Status ?? false
                }).OrderByDescending(c=>c.Meaning).ToList(),
                JourneyPath = journeyPath,
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

        [HttpGet("create-account/check-answers", Name = RouteNames.ConfirmAccountDetails)]
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

        [HttpPost("create-account/check-answers", Name = RouteNames.ConfirmAccountDetails)]
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

        [HttpGet("email", Name = RouteNames.Email)]
        public IActionResult Email(UserJourneyPath journeyPath = UserJourneyPath.ConfirmAccountDetails)
        {
            return View(new EmailViewModel { JourneyPath = journeyPath });
        }

        [HttpGet]
        [Route("settings", Name = RouteNames.Settings)]
        public async Task<IActionResult> Settings()
        {
            var accountDetails = await mediator.Send(new GetSettingsQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new SettingsViewModel
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
                HasAnsweredEqualityQuestions = accountDetails.HasAnsweredEqualityQuestions,
                CandidatePreferences = accountDetails.CandidatePreferences.Select(cp => new SettingsViewModel.CandidatePreference
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

        [HttpGet("change-notification-preferences", Name = RouteNames.ChangeNotificationPreferences)]
        public async Task<IActionResult> ChangeNotificationPreferences()
        {
            var candidatePreferences = await mediator.Send(new GetCandidatePreferencesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new ChangeNotificationPreferencesViewModel
            {
                CandidatePreferences = candidatePreferences.CandidatePreferences.Select(cp => new ChangeNotificationPreferencesViewModel.CandidatePreference
                {
                    Meaning = cp.PreferenceMeaning,
                    EmailPreference = cp.ContactMethodsAndStatus?.Where(x => x.ContactMethod == CandidatePreferencesConstants.ContactMethodEmail).FirstOrDefault()?.Status ?? false,
                }).ToList(),
            };

            return View(model);
        }
    }
}
