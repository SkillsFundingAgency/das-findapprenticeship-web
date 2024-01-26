using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.Apply;
using System;
using SFA.DAS.FAA.Web.AppStart;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/jobs/", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public IActionResult Get([FromRoute] Guid applicationId)
        {
            return View(ViewPath, new AddWorkHistoryViewModel
            {
                ApplicationId = applicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
            });
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public async Task<IActionResult> Post(AddWorkHistoryViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.AddJob))
            {
                ModelState.AddModelError(nameof(viewModel.AddJob), "Select if you want to add any jobs");
                viewModel.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId });
                return View(ViewPath, viewModel);
            }

            var command = new UpdateApplicationCommand
            {
                CandidateId =  Guid.Parse(User.Claims.First(c=>c.Type.Equals(CustomClaims.CandidateId)).Value),
                ApplicationId = viewModel.ApplicationId,
                WorkHistorySectionStatus = viewModel.AddJob is "Yes" ? SectionStatus.InProgress : SectionStatus.Completed
            };

            await mediator.Send(command);

            return viewModel.AddJob.Equals("Yes")
                ? RedirectToRoute(RouteNames.ApplyApprenticeship.AddJob, new AddJobRequest{ ApplicationId = request.ApplicationId })
                : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/jobs/add", Name = RouteNames.ApplyApprenticeship.AddJob)]
        public IActionResult GetAddAJob(AddJobRequest request)
        {
            //ModelState.Clear();
            //request.BackLinkUrl = Url.RouteUrl(RouteNames.Apply,
                //new GetIndexRequest { ApplicationId = request.ApplicationId });

            var viewModel = new AddJobViewModel
            {
                ApplicationId = request.ApplicationId
            };
            return View("~/Views/apply/workhistory/AddJob.cshtml", viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/add", Name = RouteNames.ApplyApprenticeship.AddJob)]
        public IActionResult PostAddAJob(AddJobPostRequest request)
        {
            return RedirectToRoute(RouteNames.ApplyApprenticeship.AddJob);
        }
    }
}