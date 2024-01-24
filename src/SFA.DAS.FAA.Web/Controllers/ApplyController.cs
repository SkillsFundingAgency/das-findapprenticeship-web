using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("applications/{applicationId}", Name = RouteNames.Apply)]
    public class ApplyController(IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        public async Task<IActionResult> Index(GetIndexRequest request)
        {
            var query = new GetIndexQuery
            {
                ApplicationId = request.ApplicationId,
                CandidateId = Guid.Parse("1DD26689-2997-4AEC-8FAF-62D4CE9F2155") //to be sourced from claims or similar following auth.
            };

            var result = await mediator.Send(query);
            var viewModel = IndexViewModel.Map(dateTimeService, request, result);
            return View(viewModel);
        }
    }
}
