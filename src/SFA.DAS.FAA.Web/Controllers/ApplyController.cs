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
                VacancyTitle = result.VacancyTitle,
                EmployerName = result.EmployerName,
                ApplicationId = applicationId
            };

            return View(model);
        }
    }
}
