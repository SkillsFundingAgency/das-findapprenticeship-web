using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Models.Apply;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;

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
                CandidateId = Guid.Parse(User.Claims.First(c=>c.Type.Equals(CustomClaims.CandidateId)).Value)  
            };

            var result = await mediator.Send(query);
            var viewModel = IndexViewModel.Map(dateTimeService, request, result);
            return View(viewModel);
        }
    }
}
