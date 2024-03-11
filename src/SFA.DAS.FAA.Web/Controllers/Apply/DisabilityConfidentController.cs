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
        [HttpGet]
        [Route("apply/{applicationId}/disability-confident", Name = RouteNames.ApplyApprenticeship.DisabilityConfident)]
        public async Task<IActionResult> Get(Guid applicationId, [FromRoute] bool isEdit = false)
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

            return View("~/Views/Apply/DisabilityConfident/Index.cshtml", viewModel);
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

                return View("~/Views/Apply/DisabilityConfident/Index.cshtml", viewModel);
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
    }
}
