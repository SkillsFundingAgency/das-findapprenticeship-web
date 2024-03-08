using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]

    public class DisabilityConfidentController(IMediator mediator) : Controller
    {
        [HttpGet]
        [Route("apply/{applicationId}/disability-confident", Name = RouteNames.ApplyApprenticeship.DisabilityConfident)]
        public async Task<IActionResult> Get(Guid applicationId)
        {
            var result = await mediator.Send(new GetDisabilityConfidentQuery
            {
                ApplicationId = applicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
            });

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
            return View("~/Views/Apply/DisabilityConfident/Index.cshtml", viewModel);
        }
    }
}
