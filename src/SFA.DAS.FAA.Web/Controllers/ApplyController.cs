using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("applications/{applicationId}", Name = RouteNames.Apply)]
    public class ApplyController(IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
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
                ? RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityFlow, new { model.ApplicationId })
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
