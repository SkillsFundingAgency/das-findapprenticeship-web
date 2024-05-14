using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Applications.Withdraw;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class ApplicationsController(IMediator mediator, IDateTimeService dateTimeService, ICacheStorageService cacheStorageService) : Controller
    {
        [Route("applications", Name = RouteNames.Applications.ViewApplications)]
        public async Task<IActionResult> Index(ApplicationsTab tab = ApplicationsTab.Started)
        {
            var result = await mediator.Send(new GetIndexQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Status = tab.ToApplicationStatus()
            });

            var viewModel = IndexViewModel.Map(tab, result, dateTimeService);

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
    }
}
