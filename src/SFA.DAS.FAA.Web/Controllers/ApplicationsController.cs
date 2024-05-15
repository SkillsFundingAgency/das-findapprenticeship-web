using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class ApplicationsController(IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        [Route("applications", Name = RouteNames.Applications.ViewApplications)]
        public async Task<IActionResult> Index(ApplicationsTab tab = ApplicationsTab.Started)
        {
            var result = await mediator.Send(new GetIndexQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                Status = tab.ToApplicationStatus()
            });

            var viewModel = IndexViewModel.Map(tab, result, dateTimeService);

            return View(viewModel);
        }

        [Route("applications/{applicationId}/delete", Name = RouteNames.Applications.DeleteApplication)]
        public IActionResult Delete(Guid applicationId)
        {
            return Ok("Delete application placeholder");
        }

    }
}
