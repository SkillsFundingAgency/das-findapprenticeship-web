using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.Apply;
using System;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/jobs/", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public IActionResult Get([FromRoute] Guid applicationId)
        {
            return View(ViewPath, new AddWorkHistoryRequest
            {
                ApplicationId = applicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
            });
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public async Task<IActionResult> Post(AddWorkHistoryRequest request)
        {
            if (string.IsNullOrEmpty(request.AddJob))
            {
                ModelState.AddModelError(nameof(request.AddJob), "Select if you want to add any jobs");
                request.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { request.ApplicationId });
                return View(ViewPath, request);
            }

            var command = new UpdateApplicationCommand
            {
                CandidateId = Guid.Parse("1DD26689-2997-4AEC-8FAF-62D4CE9F2155"), //to be sourced from claims or similar following auth.
                ApplicationId = request.ApplicationId,
                WorkHistorySectionStatus = request.AddJob is "Yes" ? SectionStatus.InProgress : SectionStatus.Completed
            };

            await mediator.Send(command);

            return request.AddJob.Equals("Yes")
                ? RedirectToRoute("/") //TODO: Redirect the user to Add Job Page.
                : RedirectToRoute(RouteNames.Apply, new { request.ApplicationId });
        }
    }
}