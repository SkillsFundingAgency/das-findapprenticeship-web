using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("applications", Name = RouteNames.Applications.ViewApplications)]
    public class ApplicationsController(IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        public async Task<IActionResult> Index(ApplicationsTab tab = ApplicationsTab.Started)
        {
            var result = await mediator.Send(new GetIndexQuery
            {
                CandidateId = User.Claims.CandidateId(),
                Status = tab.ToApplicationStatus()
            });

            var viewModel = IndexViewModel.Create(tab, result, dateTimeService);

            return View(viewModel);
        }

        [Route("applications/{applicationId}/delete", Name = RouteNames.Applications.DeleteApplication)]
        public IActionResult Delete(Guid applicationId)
        {
            return Ok("Delete application placeholder");
        }

    }
}
