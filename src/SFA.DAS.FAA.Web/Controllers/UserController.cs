using CreateAccount.GetAddressesByPostcode;
using CreateAccount.GetCandidateAccountDetails;
using CreateAccount.GetCandidateDateOfBirth;
using CreateAccount.GetCandidateName;
using CreateAccount.GetCandidatePhoneNumber;
using CreateAccount.GetCandidatePostcodeAddress;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidateStatus;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CheckAnswers;
using SFA.DAS.FAA.Application.Commands.CreateAccount.ManuallyEnteredAddress;
using SFA.DAS.FAA.Application.Commands.CreateAccount.PhoneNumber;
using SFA.DAS.FAA.Application.Commands.CreateAccount.SelectedAddress;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserDateOfBirth;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserName;
using SFA.DAS.FAA.Application.Commands.SavedSearches.PostDeleteSavedSearch;
using SFA.DAS.FAA.Application.Commands.User.PostAccountDeletion;
using SFA.DAS.FAA.Application.Constants;
using SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
using SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform;
using SFA.DAS.FAA.Application.Queries.User.GetSavedSearch;
using SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;
using SFA.DAS.FAA.Application.Queries.User.GetSettings;
using SFA.DAS.FAA.Web.Attributes;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("")]
    [AllowIncompleteAccountAccess]
    public class UserController(
        IMediator mediator,
        ICacheStorageService cacheStorageService,
        IConfiguration configuration,
        IOptions<Domain.Configuration.FindAnApprenticeship> faaConfiguration,
        IOidcService oidcService)
        : Controller
    {
        [HttpGet]
        [Route("create-account", Name = RouteNames.CreateAccount)]
        public async Task<IActionResult> CreateAccount([FromQuery] string returnUrl)
        {
            var result = await mediator.Send(new GetInformQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            if (result.IsAccountCreated)
            {
                return RedirectToRoute(RouteNames.ServiceStartDefault);
            }
            
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                cacheStorageService.Set($"{User.Claims.GovIdentifier()}-{CacheKeys.CreateAccountReturnUrl}", returnUrl);
            }

            return View();
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

            return RedirectToRoute(model.RedirectRoute, new { journeyPath = model.JourneyPath });
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

            return RedirectToRoute(model.RedirectRoute, new { journeyPath = model.JourneyPath });
        }

        [HttpGet("postcode-address", Name = RouteNames.PostcodeAddress)]
        public async Task<IActionResult> PostcodeAddress(string? postcode, UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var result = await mediator.Send(new GetCandidatePostcodeQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });
            var model = new PostcodeAddressViewModel
            {
                Postcode = postcode ?? result.Postcode,
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
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return RedirectToRoute(RouteNames.PostcodeAddress, new { postcode });
            }

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
                : RedirectToRoute(model.RedirectRoute, new { journeyPath = model.JourneyPath });
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
                : RedirectToRoute(model.RedirectRoute, new { journeyPath = model.JourneyPath });
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
                : RedirectToRoute(model.RedirectRoute, new { journeyPath = model.JourneyPath });
        }

        [HttpGet("notification-preferences", Name = RouteNames.NotificationPreferences)]
        public async Task<IActionResult> NotificationPreferences(UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
        {
            var candidatePreferences = await mediator.Send(new GetCandidatePreferencesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            var model = new NotificationPreferencesViewModel
            {
                JourneyPath = journeyPath,
                UnfinishedApplicationReminders = candidatePreferences.UnfinishedApplicationReminders
            };
            return View(model);
        }

        [HttpPost("notification-preferences", Name = RouteNames.NotificationPreferences)]
        public async Task<IActionResult> NotificationPreferences(NotificationPreferencesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.Send(new UpsertCandidatePreferencesCommand
            {
                CandidateEmail = User.Claims.Email()!,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                UnfinishedApplicationReminders = model.UnfinishedApplicationReminders ?? false
            });

            return RedirectToRoute(model.RedirectRoute, new { journeyPath = model.JourneyPath });
        }

        [HttpGet("create-account/check-answers", Name = RouteNames.ConfirmAccountDetails)]
        public async Task<IActionResult> CheckAnswers(UserJourneyPath journeyPath = UserJourneyPath.CreateAccount)
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
                JourneyPath = journeyPath,
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
            await cacheStorageService.Remove($"{User.Claims.GovIdentifier()}-{CacheKeys.CreateAccountReturnUrl}");

            if (model.JourneyPath == UserJourneyPath.CreateAccount) await cacheStorageService.Set($"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}", true);

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToRoute(RouteNames.ServiceStartDefault);
        }

        [HttpGet]
        [Route("settings", Name = RouteNames.Settings)]
        public async Task<IActionResult> Settings()
        {
            var email = await GetEmailFromUserInfoEndpoint();
            var accountDetails = await mediator.Send(new GetSettingsQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Email = email?.Email
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
                    Meaning = cp.Meaning,
                    Hint = cp.Hint,
                    EmailPreference = cp.EmailPreference,
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        [Route("saved-searches", Name = RouteNames.SavedSearches)]
        public async Task<IActionResult> GetSavedSearches([FromQuery] Guid? deleted, CancellationToken cancellationToken)
        {
            string? deleteSavedSearchTitle = null;
            if (deleted is not null)
            {
                deleteSavedSearchTitle = await cacheStorageService.Get<string?>($"{(Guid)User.Claims.CandidateId()!}-{deleted}-savedsearch");
            }
            
            var result = await mediator.Send(new GetSavedSearchesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!
            }, cancellationToken);

            var model = new SavedSearchesViewModel(result.SavedSearches.Select(x => SavedSearchViewModel.From(x, result.Routes)).ToList(), Constants.SavedSearchLimit, deleted is not null, deleteSavedSearchTitle);
            return View(model);
        }
        
        [HttpGet]
        [Route("saved-searches/{savedSearchId}/delete", Name = RouteNames.ConfirmDeleteSavedSearch)]
        public async Task<IActionResult> ConfirmDeleteSavedSearch([FromRoute] Guid savedSearchId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSavedSearchQuery((Guid)User.Claims.CandidateId()!, savedSearchId), cancellationToken);
            if (result.SavedSearch is null)
            {
                return NotFound();
            }

            var model = SavedSearchViewModel.From(result.SavedSearch, result.Routes, null, true);
            await cacheStorageService.Set($"{(Guid)User.Claims.CandidateId()!}-{savedSearchId}-savedsearch", model.Title, 5, 5);
            return View(model); 
        }
        
        [HttpPost]
        [Route("saved-searches/{savedSearchId}/delete", Name = RouteNames.DeleteSavedSearch)]
        public async Task<IActionResult> DeleteSavedSearch([FromRoute] Guid savedSearchId, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteSavedSearchCommand
            {
                Id = savedSearchId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            }, cancellationToken);

            return RedirectToRoute(RouteNames.SavedSearches, new { deleted = savedSearchId });
        }

        private async Task<GovUkUser?> GetEmailFromUserInfoEndpoint()
        {
            _ = bool.TryParse(configuration["StubAuth"], out var stubAuth);
            if (stubAuth)
            {
                return null;
            }
            var token = await HttpContext.GetTokenAsync("access_token");
            var email = await oidcService.GetAccountDetails(token);
            return email;
        }

        
		

       

        [HttpGet("finish-account-setup", Name = RouteNames.FinishAccountSetup)]
        public IActionResult FinishAccountSetup()
        {
            return View();
        }

        [HttpGet("email", Name = RouteNames.Email)]
        public IActionResult Email(UserJourneyPath journeyPath = UserJourneyPath.ConfirmAccountDetails)
        {
            return View(new EmailViewModel(configuration["ResourceEnvironmentName"]!.Equals("PRD", StringComparison.CurrentCultureIgnoreCase)) { JourneyPath = journeyPath });
        }

        [HttpGet]
        [Route("confirm-account-deletion", Name = RouteNames.ConfirmAccountDelete)]
        public IActionResult ConfirmAccountDeletion()
        {
            return View();
        }

        [HttpGet]
        [Route("account-deletion-withdraw-applications", Name = RouteNames.AccountDeleteWithDrawApplication)]
        public async Task<IActionResult> AccountDeletionWithDrawApplications()
        {
            var result = await mediator.Send(new GetAccountDeletionApplicationsToWithdrawQuery((Guid)User.Claims.CandidateId()!)
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
            });

            return result.SubmittedApplications.Count == 0
                ? RedirectToRoute(RouteNames.AccountDelete, new { journeyPath = AccountDeletionViewModel.RouthPath.ConfirmAccountDeletion })
                : View((AccountDeletionWithDrawApplicationsViewModel) result);
        }

        [HttpGet]
        [Route("account-delete", Name = RouteNames.AccountDelete)]
        public IActionResult AccountDeletion(AccountDeletionViewModel.RouthPath journeyPath = AccountDeletionViewModel.RouthPath.ConfirmAccountDeletion)
        {
            var model = new AccountDeletionViewModel
            {
                JourneyPath = journeyPath
            };

            return View(model);
        }

        [HttpPost]
        [Route("account-delete", Name = RouteNames.AccountDelete)]
        public async Task<IActionResult> AccountDeletion(AccountDeletionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            };

            if (!model.Email.Equals(User.Claims.Email(), StringComparison.CurrentCultureIgnoreCase))
            {
                ModelState.AddModelError(nameof(AccountDeletionViewModel.Email), "This is not the email address you use with Find an apprenticeship. Check your email address and try again");
                return View(model);
            }

            await mediator.Send(new AccountDeletionCommand((Guid)User.Claims.CandidateId()!));

            TempData[CacheKeys.AccountDeleted] = "true";

            return RedirectToRoute(RouteNames.SignOut);
        }
    }
}
