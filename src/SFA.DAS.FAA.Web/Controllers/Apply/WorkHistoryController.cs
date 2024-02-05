using MediatR;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;
using System;
using SFA.DAS.FAA.Application.Commands.WorkHistory.UpdateJob;
using SFA.DAS.FAA.Application.Queries.Apply.GetJob;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";
        private const string SummaryViewPath = "~/Views/apply/workhistory/Summary.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/jobs", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId)
        {
            var result = await mediator.Send(new GetJobsQuery
            {
                ApplicationId = applicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
            });

            var viewModel = new JobsViewModel
            {
                ApplicationId = applicationId,
                JobHistory = result.Jobs.Select(x => (JobsViewModel.Job)x).ToList(),
                ShowJobHistory = result.Jobs.Any()
            };

            return View(ViewPath, viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public async Task<IActionResult> Post(JobsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetJobsQuery
                {
                    ApplicationId = viewModel.ApplicationId,
                    CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
                });

                viewModel = new JobsViewModel
                {
                    ApplicationId = viewModel.ApplicationId,
                    JobHistory = result.Jobs.Select(x => (JobsViewModel.Job)x).ToList(),
                    ShowJobHistory = result.Jobs.Any()
                };

                return View(ViewPath, viewModel);
            }

            if (viewModel.ShowJobHistory)
            {
                var completeSectionCommand = new UpdateApplicationCommand
                {
                    CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                    ApplicationId = viewModel.ApplicationId,
                    WorkHistorySectionStatus = viewModel.IsSectionCompleted.Value ? SectionStatus.Completed : SectionStatus.InProgress
                };

                await mediator.Send(completeSectionCommand);

                return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
            }

            var command = new UpdateApplicationCommand
            {
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                ApplicationId = viewModel.ApplicationId,
                WorkHistorySectionStatus = viewModel.DoYouWantToAddAnyJobs.Value ? SectionStatus.InProgress : SectionStatus.Completed
            };

            await mediator.Send(command);

            return viewModel.DoYouWantToAddAnyJobs.Value
                ? RedirectToRoute(RouteNames.ApplyApprenticeship.AddJob, new { viewModel.ApplicationId })
                : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/jobs/add", Name = RouteNames.ApplyApprenticeship.AddJob)]
        public IActionResult GetAddAJob([FromRoute] Guid applicationId)
        {
            var viewModel = new AddJobViewModel
            {
                ApplicationId = applicationId
            };

            return View("~/Views/apply/workhistory/AddJob.cshtml", viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/add", Name = RouteNames.ApplyApprenticeship.AddJob)]
        public async Task<IActionResult> PostAddAJob(AddJobViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/apply/workhistory/AddJob.cshtml", request);
            }

            var command = new AddJobCommand
            {
                ApplicationId = request.ApplicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                EmployerName = request.EmployerName,
                JobDescription = request.JobDescription,
                JobTitle = request.JobTitle,
                StartDate = request.StartDate.DateTimeValue.Value,
                EndDate = request.IsCurrentRole is true ? null : request.EndDate?.DateTimeValue
            };

            await mediator.Send(command);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.Jobs, new { request.ApplicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/jobs/{jobId}", Name = RouteNames.ApplyApprenticeship.EditJob)]
        public async Task<IActionResult> Edit([FromRoute] Guid applicationId, Guid jobId)
        {
            var result = await mediator.Send(new GetJobQuery
            {
                ApplicationId = applicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                JobId = jobId
            });

            var viewModel = (EditJobViewModel)result;

            return View("~/Views/apply/workhistory/EditJob.cshtml", viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/{jobId}", Name = RouteNames.ApplyApprenticeship.EditJob)]
        public async Task<IActionResult> PostEditAJob(EditJobViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/apply/workhistory/EditJob.cshtml", request);
            }

            var command = new UpdateJobCommand
            {
                JobId = request.JobId,
                ApplicationId = request.ApplicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                EmployerName = request.EmployerName,
                JobDescription = request.JobDescription,
                JobTitle = request.JobTitle,
                StartDate = request.StartDate.DateTimeValue.Value,
                EndDate = request.IsCurrentRole is true ? null : request.EndDate?.DateTimeValue
            };

            await mediator.Send(command);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.Jobs, new { request.ApplicationId });
        }

        [Route("apply/{applicationId}/jobs/delete", Name = RouteNames.ApplyApprenticeship.DeleteJob)]
        public IActionResult DeleteJob([FromRoute] Guid applicationId)
        {
               // JobId = request.JobId,
            return View("~/Views/apply/workhistory/DeleteJob.cshtml");
        }

    }
}