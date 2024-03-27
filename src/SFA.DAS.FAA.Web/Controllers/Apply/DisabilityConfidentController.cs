using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

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
                    new { ApplicationId = applicationId, isEdit });
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
        public async Task<IActionResult> Post(Guid applicationId, DisabilityConfidentViewModel viewModel, [FromQuery] bool isEdit = false)
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
                new { ApplicationId = applicationId, isEdit });
        }

        [HttpGet]
        [Route("apply/{applicationId}/disability-confident/confirm", Name = RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation)]
        public async Task<IActionResult> GetSummary([FromRoute] Guid applicationId, [FromQuery] bool isEdit = false)
        {
            var result = await mediator.Send(new GetDisabilityConfidentDetailsQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = new DisabilityConfidentSummaryViewModel
            {
                ApplicationId = applicationId,
                BackLinkUrl = isEdit 
                    ? Url.RouteUrl(RouteNames.ApplyApprenticeship.DisabilityConfident, new { applicationId, isEdit })
                    : Url.RouteUrl(RouteNames.Apply, new { applicationId }),
                IsApplyUnderDisabilityConfidentSchemeRequired = result.ApplyUnderDisabilityConfidentScheme ?? false,
                IsSectionCompleted = result.IsSectionCompleted
            };

            return View(SummaryViewPath, viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/disability-confident/confirm", Name = RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation)]
        public async Task<IActionResult> PostSummary([FromRoute] Guid applicationId, DisabilityConfidentSummaryViewModel viewModel, [FromQuery] bool isEdit = false)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetDisabilityConfidentDetailsQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                viewModel = new DisabilityConfidentSummaryViewModel
                {
                    ApplicationId = applicationId,
                    BackLinkUrl = isEdit
                        ? Url.RouteUrl(RouteNames.ApplyApprenticeship.DisabilityConfident, new { applicationId, isEdit })
                        : Url.RouteUrl(RouteNames.Apply, new { applicationId }),
                    IsApplyUnderDisabilityConfidentSchemeRequired = result.ApplyUnderDisabilityConfidentScheme ?? false,
                    IsSectionCompleted = result.IsSectionCompleted
                };

                return View(SummaryViewPath, viewModel);
            }

            var updateCommand = new UpdateDisabilityConfidenceApplicationCommand
            {
                ApplicationId = viewModel.ApplicationId,
                CandidateId = User.Claims.CandidateId(),
                IsSectionCompleted = viewModel.IsSectionCompleted!.Value
            };

            await mediator.Send(updateCommand);

            return RedirectToRoute(RouteNames.Apply, new { applicationId });
        }
    }
}
