using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Applications.Withdraw;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class ApplicationsController(IMediator mediator, IDateTimeService dateTimeService, ICacheStorageService cacheStorageService, INotificationCountService notificationCountService) : Controller
    {
        private const string ApplicationPreviewViewPath = "~/Views/applications/ViewApplication.cshtml";

        [Route("applications", Name = RouteNames.Applications.ViewApplications)]
        public async Task<IActionResult> Index(ApplicationsTab tab = ApplicationsTab.Started, bool showEqualityQuestionsBanner = false)
        {
            var bannerMessage = await cacheStorageService.Get<string>($"{User.Claims.GovIdentifier()}-VacancyWithdrawn");
            if (!string.IsNullOrEmpty(bannerMessage))
            {
                await cacheStorageService.Remove($"{User.Claims.GovIdentifier()}-VacancyWithdrawn");
            }

            var applicationSubmittedBannerMessage = await cacheStorageService.Get<string>($"{User.Claims.GovIdentifier()}-ApplicationSubmitted");
            if (!string.IsNullOrEmpty(applicationSubmittedBannerMessage))
            {
                await cacheStorageService.Remove($"{User.Claims.GovIdentifier()}-ApplicationSubmitted");
            }

            var result = await mediator.Send(new GetIndexQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Status = tab.ToApplicationStatus()
            });


            var newSuccessfulApplicationsCountTask = notificationCountService.GetUnreadApplicationCount(
                (Guid) User.Claims.CandidateId()!,
                ApplicationStatus.Successful);

            var newUnsuccessfulApplicationsCountTask = notificationCountService.GetUnreadApplicationCount(
                (Guid)User.Claims.CandidateId()!,
                ApplicationStatus.Unsuccessful);

            await Task.WhenAll(newSuccessfulApplicationsCountTask, newUnsuccessfulApplicationsCountTask);

            var newSuccessfulApplicationsCount = newSuccessfulApplicationsCountTask.Result;
            var newUnsuccessfulApplicationsCount = newUnsuccessfulApplicationsCountTask.Result;

            var viewModel = IndexViewModel.Map(tab, result, dateTimeService);
            viewModel.WithdrawnBannerMessage = bannerMessage;
            viewModel.ApplicationSubmittedBannerMessage = applicationSubmittedBannerMessage;
            viewModel.ShowEqualityQuestionsBannerMessage = showEqualityQuestionsBanner;
            viewModel.NewSuccessfulApplicationsCount = newSuccessfulApplicationsCount.GetCountLabel();
            viewModel.NewUnsuccessfulApplicationsCount = newUnsuccessfulApplicationsCount.GetCountLabel();

            switch (tab)
            {
                case ApplicationsTab.Successful or ApplicationsTab.Unsuccessful:
                    switch (tab)
                    {
                        case ApplicationsTab.Successful:
                            await notificationCountService.MarkAllNotificationsAsRead((Guid)User.Claims.CandidateId()!, ApplicationStatus.Successful);
                            break;
                        case ApplicationsTab.Unsuccessful:
                            await notificationCountService.MarkAllNotificationsAsRead((Guid)User.Claims.CandidateId()!, ApplicationStatus.Unsuccessful);
                            break;
                    }
                    break;
            }
            return View(viewModel);
        }

        [Route("applications/{applicationId}/delete", Name = RouteNames.Applications.DeleteApplication)]
        public IActionResult Delete(Guid applicationId)
        {
            return Ok("Delete application placeholder");
        }

        [Route("applications/{applicationId}/withdraw", Name = RouteNames.Applications.WithdrawApplicationGet)]
        public async Task<IActionResult> Withdraw([FromRoute]Guid applicationId)
        {
            var result = await mediator.Send(new GetWithdrawApplicationQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                ApplicationId = applicationId
            });

            var viewModel = new WithdrawApplicationViewModel(dateTimeService, result);
            
            return View(viewModel);
        }
        
        [HttpPost]
        [Route("applications/{applicationId}/withdraw", Name = RouteNames.Applications.WithdrawApplicationPost)]
        public async Task<IActionResult> Withdraw(PostWithdrawApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetWithdrawApplicationQuery
                {
                    CandidateId = (Guid)User.Claims.CandidateId()!,
                    ApplicationId = model.ApplicationId
                });

                var viewModel = new WithdrawApplicationViewModel(dateTimeService, result);
                return View(viewModel);
            }

            if (model.WithdrawApplication.HasValue && model.WithdrawApplication.Value)
            {
                await mediator.Send(new WithdrawApplicationCommand
                {
                    ApplicationId = model.ApplicationId,
                    CandidateId = (Guid)User.Claims.CandidateId()!
                });
                await cacheStorageService.Set($"{User.Claims.GovIdentifier()}-VacancyWithdrawn", $"Application withdrawn for {model.AdvertTitle} at {model.EmployerName}.", 1, 1);
            }
            
            return RedirectToRoute(RouteNames.Applications.ViewApplications,new {tab = ApplicationsTab.Submitted});
        }
        [HttpGet]
        [Route("applications/{applicationId}/view", Name = RouteNames.Applications.ViewApplication)]
        public async Task<IActionResult> View([FromRoute] Guid applicationId)
        {
            var query = new GetApplicationViewQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()!
            };
            var result = await mediator.Send(query);

            var viewModel = (ApplicationViewModel)result;
            viewModel.ApplicationId = applicationId;

            return View(ApplicationPreviewViewPath, viewModel);
        }
    }
}
