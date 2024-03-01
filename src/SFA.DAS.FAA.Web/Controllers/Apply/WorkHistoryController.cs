using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Application.Commands.WorkHistory.UpdateJob;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteJob;
using SFA.DAS.FAA.Application.Queries.Apply.GetJob;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using System;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/Index.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/jobs", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId)
        {
            var result = await mediator.Send(new GetJobsQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = new JobsViewModel
            {
                ApplicationId = applicationId,
                DoYouWantToAddAnyJobs = result.Jobs.Count == 0 && result.IsSectionCompleted is true ? false : null,
                IsSectionCompleted = result.IsSectionCompleted,
                JobHistory = result.Jobs.Select(x => (JobsViewModel.Job)x).ToList(),
                ShowJobHistory = result.Jobs.Count != 0
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
                    CandidateId = User.Claims.CandidateId()
                });

                viewModel = new JobsViewModel
                {
                    ApplicationId = viewModel.ApplicationId,
                    DoYouWantToAddAnyJobs = result.Jobs.Count == 0 && result.IsSectionCompleted is true ? false : null,
                    IsSectionCompleted = result.IsSectionCompleted,
                    JobHistory = result.Jobs.Select(x => (JobsViewModel.Job)x).ToList(),
                    ShowJobHistory = result.Jobs.Count != 0
                };

                return View(ViewPath, viewModel);
            }

            if (viewModel.ShowJobHistory)
            {
                var completeSectionCommand = new UpdateWorkHistoryApplicationCommand
                {
                    CandidateId = User.Claims.CandidateId(),
                    ApplicationId = viewModel.ApplicationId,
                    WorkHistorySectionStatus = viewModel.IsSectionCompleted.Value ? SectionStatus.Completed : SectionStatus.InProgress
                };

                await mediator.Send(completeSectionCommand);

                return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
            }

            var command = new UpdateWorkHistoryApplicationCommand
            {
                CandidateId = User.Claims.CandidateId(),
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
                CandidateId = User.Claims.CandidateId(),
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
                CandidateId = User.Claims.CandidateId(),
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
                CandidateId = User.Claims.CandidateId(),
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
        [Route("apply/{applicationId}/jobs/{jobId}/delete", Name = RouteNames.ApplyApprenticeship.DeleteJob)]
        public async Task<IActionResult> GetDeleteJob([FromRoute] Guid applicationId, Guid jobId)
        {
            var result = await mediator.Send(new GetDeleteJobQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId(),
                JobId = jobId
            });

            var viewModel = (DeleteJobViewModel)result;

            return View("~/Views/apply/workhistory/DeleteJob.cshtml", viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/{jobId}/delete", Name = RouteNames.ApplyApprenticeship.DeleteJob)]
        public async Task<IActionResult> PostDeleteJob(DeleteJobViewModel model)
        {
            try
            {
                var command = new PostDeleteJobCommand
                {
                    CandidateId = User.Claims.CandidateId(),
                    ApplicationId = model.ApplicationId,
                    JobId = model.JobId
                };
                await mediator.Send(command);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(DeleteJobViewModel), "There's been a problem");
                return View("~/Views/apply/workhistory/DeleteJob.cshtml");
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.Jobs, new { model.ApplicationId });

        }
    }
}