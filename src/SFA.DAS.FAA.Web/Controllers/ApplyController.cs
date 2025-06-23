using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.SubmitApplication;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;
using IndexViewModel = SFA.DAS.FAA.Web.Models.Apply.IndexViewModel;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("applications/{applicationId}", Name = RouteNames.Apply)]
    public class ApplyController(
        IMediator mediator,
        ICacheStorageService cacheStorageService,
        IDateTimeService dateTimeService) : Controller
    {
        private const string PreviewViewPath = "~/Views/apply/Preview.cshtml";

        public async Task<IActionResult> Index(GetIndexRequest request)
        {
            var query = new GetIndexQuery
            {
                ApplicationId = request.ApplicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            };

            var result = await mediator.Send(query);

            var viewModel = IndexViewModel.Map(dateTimeService, request, result);
            viewModel.PageBackLink =
	            Request.Headers.Referer.FirstOrDefault() ?? Url.RouteUrl(RouteNames.Applications.ViewApplications);
			return View(viewModel);
        }

        [HttpGet("preview", Name = RouteNames.ApplyApprenticeship.Preview)]
        public async Task<IActionResult> Preview([FromRoute] Guid applicationId)
        {
            var query = new GetApplicationSummaryQuery
            {
                ApplicationId = applicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            };
            var result = await mediator.Send(query);
            
            var viewModel = (ApplicationSummaryViewModel)result;
            viewModel.ApplicationId = applicationId;

            return
                !viewModel.IsApplicationComplete
                ? RedirectToRoute(RouteNames.Apply, new { applicationId })
                : View(PreviewViewPath, viewModel);
        }

        [HttpPost("preview", Name = RouteNames.ApplyApprenticeship.Preview)]
        public async Task<IActionResult> Preview([FromRoute] Guid applicationId, ApplicationSummaryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var query = new GetApplicationSummaryQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = (Guid)User.Claims.CandidateId()!
                };

                var result = await mediator.Send(query);
                viewModel = result;
                viewModel.ApplicationId = applicationId;
                return View(PreviewViewPath, viewModel);
            }

            await mediator.Send(new SubmitApplicationCommand
            {
                ApplicationId = applicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            return RedirectToRoute(RouteNames.ApplyApprenticeship.ApplicationSubmitted, new {applicationId});
        }

        [HttpPost]
        public IActionResult Index([FromRoute] Guid applicationId)
        {
            return RedirectToRoute(RouteNames.ApplyApprenticeship.ApplicationSubmitted, new { applicationId });
        }

        [HttpGet]
        [Route("application-submitted", Name = RouteNames.ApplyApprenticeship.ApplicationSubmitted)]
        public async Task<IActionResult> ApplicationSubmitted([FromRoute] Guid applicationId)
        {
            var query = new GetApplicationSubmittedQuery
            {
                ApplicationId = applicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            };

            var result = await mediator.Send(query);

            var model = new ApplicationSubmittedViewModel
            {
                VacancyInfo = result,
                ApplicationId = applicationId,
                ClosedDate = VacancyDetailsHelperService.GetClosedDate(result.ClosedDate),
                IsVacancyClosedEarly = result.ClosedDate.HasValue || result.ClosingDate < DateTime.UtcNow,
            };

            return View(model);
        }

        [HttpPost]
        [Route("application-submitted", Name = RouteNames.ApplyApprenticeship.ApplicationSubmitted)]
        public async Task<IActionResult> ApplicationSubmitted(ApplicationSubmittedViewModel model)
        {
            var query = new GetApplicationSubmittedQuery
            {
                ApplicationId = model.ApplicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            };

            var result = await mediator.Send(query);

            if (!ModelState.IsValid)
            {
                model = new ApplicationSubmittedViewModel
                {
                    VacancyInfo = result,
                    ApplicationId = model.ApplicationId
                };

                return View(model);
            }

            if (model.AnswerEqualityQuestions is false)
            {
                await cacheStorageService.Set($"{User.Claims.GovIdentifier()}-ApplicationSubmitted", $"Your application for {result.VacancyTitle} at {result.EmployerName} has been submitted.", 1, 1);
            }

            return model.AnswerEqualityQuestions is true 
                ? RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender, new { model.ApplicationId })
                : RedirectToRoute(RouteNames.Applications.ViewApplications, new { tab = ApplicationsTab.Submitted });
        }
    }
}