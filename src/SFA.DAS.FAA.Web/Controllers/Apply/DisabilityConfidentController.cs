using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using static System.String;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class DisabilityConfidentController(IMediator mediator) : Controller
    {
        private const string IndexViewPath = "~/Views/apply/DisabilityConfident/Index.cshtml";
        private const string SummaryViewPath = "~/Views/apply/DisabilityConfident/Summary.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/disability-confident", Name = RouteNames.ApplyApprenticeship.DisabilityConfident)]
        public async Task<IActionResult> Get(Guid applicationId, [FromQuery] bool isEdit = false)
        {
            var result = await mediator.Send(new GetDisabilityConfidentQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            if (result.ApplyUnderDisabilityConfidentScheme.HasValue && !isEdit)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation,
                    new { ApplicationId = applicationId });
            }

            var viewModel = new DisabilityConfidentViewModel
            {
                ApplicationId = applicationId,
                EmployerName = result.EmployerName,
                ApplyUnderDisabilityConfidentScheme = result.ApplyUnderDisabilityConfidentScheme
            };

            return View(IndexViewPath, viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/disability-confident", Name = RouteNames.ApplyApprenticeship.DisabilityConfident)]
        public async Task<IActionResult> Post(Guid applicationId, DisabilityConfidentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetDisabilityConfidentQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                viewModel = new DisabilityConfidentViewModel
                {
                    ApplicationId = applicationId,
                    EmployerName = result.EmployerName,
                    ApplyUnderDisabilityConfidentScheme = result.ApplyUnderDisabilityConfidentScheme
                };

                return View(IndexViewPath, viewModel);
            }

            await mediator.Send(new UpdateDisabilityConfidentCommand
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId(),
                ApplyUnderDisabilityConfidentScheme = viewModel.ApplyUnderDisabilityConfidentScheme ?? false
            });

            return RedirectToRoute(RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation,
                new { ApplicationId = applicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/disability-confident/summary",
            Name = RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation)]
        public async Task<IActionResult> GetSummary([FromRoute] Guid applicationId)
        {
            var result = await mediator.Send(new GetDisabilityConfidentQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = new DisabilityConfidentSummaryViewModel
            {
                ApplicationId = applicationId,
                IsApplyUnderDisabilityConfidentSchemeRequired = result.ApplyUnderDisabilityConfidentScheme ?? false
            };

            return View(SummaryViewPath, viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/disability-confident/summary",
            Name = RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation)]
        public async Task<IActionResult> PostSummary([FromRoute] Guid applicationId, DisabilityConfidentSummaryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetDisabilityConfidentQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                viewModel = new DisabilityConfidentSummaryViewModel
                {
                    ApplicationId = applicationId,
                    IsApplyUnderDisabilityConfidentSchemeRequired = result.ApplyUnderDisabilityConfidentScheme ?? false
                };

                return View(SummaryViewPath, viewModel);
            }

            var updateCommand = new UpdateDisabilityConfidentCommand
            {
                ApplicationId = viewModel.ApplicationId,
                CandidateId = User.Claims.CandidateId(),
                ApplyUnderDisabilityConfidentScheme = viewModel.IsSectionCompleted!.Value ? true : false
            };

            await mediator.Send(updateCommand);

            return RedirectToRoute(RouteNames.Apply, new { applicationId });
        }
    }
}
