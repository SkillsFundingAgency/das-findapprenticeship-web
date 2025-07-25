using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Applications.Delete;
using SFA.DAS.FAA.Application.Commands.Applications.Withdraw;
using SFA.DAS.FAA.Application.Queries.Applications.Delete;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class ApplicationsController(IMediator mediator, IDateTimeService dateTimeService, ICacheStorageService cacheStorageService, INotificationCountService notificationCountService) : Controller
{
    private const string ApplicationPreviewViewPath = "~/Views/applications/ViewApplication.cshtml";
    private string ApplicationDeletedCacheKey => $"{User.Claims.GovIdentifier()}-application-deleted";
    
    private async Task<string?> GetAndRemoveCachedMessage(string key)
    {
        var result = await cacheStorageService.Get<string>(key);
        if (!string.IsNullOrEmpty(result))
        {
            await cacheStorageService.Remove(key);
        }

        return result;
    }

    [Route("applications", Name = RouteNames.Applications.ViewApplications)]
    public async Task<IActionResult> Index(ApplicationsTab tab = ApplicationsTab.Started, bool showEqualityQuestionsBanner = false)
    {
        var bannerMessage = await GetAndRemoveCachedMessage($"{User.Claims.GovIdentifier()}-VacancyWithdrawn"); 
        var applicationSubmittedBannerMessage = await GetAndRemoveCachedMessage($"{User.Claims.GovIdentifier()}-ApplicationSubmitted"); 
        var deletedApplicationMessage = await GetAndRemoveCachedMessage(ApplicationDeletedCacheKey); 

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
        viewModel.DeletedBannerMessage = deletedApplicationMessage;
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

    [Route("applications/{applicationId:guid}/withdraw", Name = RouteNames.Applications.WithdrawApplicationGet)]
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
        
    [HttpGet, Route("applications/{applicationId}/delete", Name = RouteNames.Applications.DeleteApplicationGet)]
    public async Task<IActionResult> ConfirmDelete([FromRoute]Guid applicationId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDeleteApplicationQuery(User.Claims.CandidateId()!.Value, applicationId), cancellationToken);
        if (result == GetDeleteApplicationQueryResult.None)
        {
            return RedirectToRoute(RouteNames.Applications.ViewApplications, new { tab = ApplicationsTab.Started });
        }

        var viewModel = new ConfirmDeleteApplicationViewModel
        {
            ApplicationId = result.ApplicationId,
            ApplicationStartDate = result.ApplicationStartDate.ToGdsDateString(),
            ApprenticeshipType = result.ApprenticeshipType,
            EmployerName = result.EmployerName!,
            VacancyTitle = result.VacancyTitle!,
            VacancyCloseDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, result.VacancyClosingDate, result.VacancyClosedDate),
            WorkLocation = result.EmployerLocationOption switch
            {
                AvailableWhere.MultipleLocations => VacancyDetailsHelperService.GetEmploymentLocationCityNames(result.Addresses),
                AvailableWhere.AcrossEngland => "Recruiting nationally",
                _ => VacancyDetailsHelperService.GetOneLocationCityName(result.Addresses.First())!
            }
        };
            
        return View(viewModel);
    }
        
    [HttpPost, Route("applications/{applicationId:guid}/delete", Name = RouteNames.Applications.DeleteApplicationPost)]
    public async Task<IActionResult> Delete([FromRoute]Guid applicationId, CancellationToken cancellationToken)
    {
        if (applicationId == Guid.Empty)
        {
            return RedirectToRoute(RouteNames.Applications.ViewApplications, new { tab = ApplicationsTab.Started });
        }
            
        var result = await mediator.Send(new DeleteApplicationCommand(User.Claims.CandidateId()!.Value, applicationId), cancellationToken);
        if (result.Success)
        {
            var message = $"Application deleted for {result.VacancyTitle} at {result.EmployerName}";
            await cacheStorageService.Set(ApplicationDeletedCacheKey, message);
        }
            
        return RedirectToRoute(RouteNames.Applications.ViewApplications, new { tab = ApplicationsTab.Started });
    }
}