using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.WhatInterestsYou;
using SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class WhatInterestsYouController(IMediator mediator) : Controller
    {
        private const string ViewName = "~/Views/apply/WhatInterestsYou/Index.cshtml";

        [Route("apply/{applicationId}/what-interests-you", Name = RouteNames.ApplyApprenticeship.WhatInterestsYou)]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId)
        {
            var result = await mediator.Send(new GetWhatInterestsYouQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = new WhatInterestsYouViewModel
            {
                ApplicationId = applicationId,
                StandardName = result.StandardName,
                EmployerName = result.EmployerName,
                AnswerText = result.AnswerText,
                IsSectionCompleted = result.IsSectionCompleted
            };

            return View(ViewName, viewModel);
        }

        [Route("apply/{applicationId}/what-interests-you", Name = RouteNames.ApplyApprenticeship.WhatInterestsYou)]
        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] Guid applicationId, WhatInterestsYouViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetWhatInterestsYouQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                var viewModel = new WhatInterestsYouViewModel
                {
                    ApplicationId = applicationId,
                    StandardName = result.StandardName,
                    EmployerName = result.EmployerName,
                    AnswerText = result.AnswerText,
                    IsSectionCompleted = result.IsSectionCompleted
                };

                return View(ViewName, viewModel);
            }

            await mediator.Send(new UpdateWhatInterestsYouCommand
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId(),
                AnswerText = model.AnswerText ?? string.Empty,
                IsComplete = model.IsSectionCompleted ?? false
            });

            return RedirectToRoute(RouteNames.Apply, new { applicationId });
        }
    }
}
