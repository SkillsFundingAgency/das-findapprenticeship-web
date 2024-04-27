using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.SubmitApplication;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.ApplicationStatus;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("applications/{applicationId}", Name = RouteNames.Apply)]
    public class ApplyController
        (IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        private const string PreviewViewPath = "~/Views/apply/Preview.cshtml";

        public async Task<IActionResult> Index(GetIndexRequest request)
        {
            var query = new GetIndexQuery
            {
                ApplicationId = request.ApplicationId,
                CandidateId = User.Claims.CandidateId()
            };

            var result = await mediator.Send(query);
            var viewModel = IndexViewModel.Map(dateTimeService, request, result);
            return View(viewModel);
        }

        [HttpGet("preview", Name = RouteNames.ApplyApprenticeship.Preview)]
        public async Task<IActionResult> Preview([FromRoute] Guid applicationId)
        {
            var query = new GetApplicationSummaryQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
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
                    CandidateId = User.Claims.CandidateId()
                };

                var result = await mediator.Send(query);
                viewModel = result;
                viewModel.ApplicationId = applicationId;
                return View(PreviewViewPath, viewModel);
            }

            await mediator.Send(new SubmitApplicationCommand
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
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
                CandidateId = User.Claims.CandidateId()
            };

            var result = await mediator.Send(query);

            var model = new ApplicationSubmittedViewModel
            {
                VacancyInfo = result,
                ApplicationId = applicationId
            };

            return View(model);
        }

        [HttpPost]
        [Route("application-submitted", Name = RouteNames.ApplyApprenticeship.ApplicationSubmitted)]
        public async Task<IActionResult> ApplicationSubmitted(ApplicationSubmittedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var query = new GetApplicationSubmittedQuery
                {
                    ApplicationId = model.ApplicationId,
                    CandidateId = User.Claims.CandidateId()
                };

                var result = await mediator.Send(query);

                model = new ApplicationSubmittedViewModel
                {
                    VacancyInfo = result,
                    ApplicationId = model.ApplicationId
                };

                return View(model);
            }

            return model.AnswerEqualityQuestions is true 
                ? RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender, new { model.ApplicationId })
                : RedirectToRoute(RouteNames.ApplyApprenticeship.ApplicationSubmittedConfirmation, new { model.ApplicationId });
        }

        [HttpGet]
        [Route("application-submitted-confirmation", Name = RouteNames.ApplyApprenticeship.ApplicationSubmittedConfirmation)]
        public async Task<IActionResult> ApplicationSubmittedConfirmation([FromRoute] Guid applicationId)
        {
            var query = new GetApplicationSubmittedQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            };

            var result = await mediator.Send(query);

            var model = new ApplicationSubmittedConfirmationViewModel
            {
                VacancyInfo = result,
            };

            return View(model);
        }
    }
}