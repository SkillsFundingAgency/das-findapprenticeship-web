using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.AddTrainingCourse;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class TrainingCoursesController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/trainingcourses/List.cshtml";

    [HttpGet]
    [Route("apply/{applicationId}/trainingcourses/", Name = RouteNames.ApplyApprenticeship.TrainingCourses)]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId)
    {
        var result = await mediator.Send(new GetTrainingCoursesQuery
        {
            ApplicationId = applicationId,
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
        });

        var viewModel = new TrainingCoursesViewModel
        {
            ApplicationId = applicationId,
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId }),
            TrainingCourses = result.TrainingCourses.Select(t => (TrainingCoursesViewModel.TrainingCourse)t).ToList(),
            ShowTrainingCoursesAchieved = result.TrainingCourses.Any()
        };

        return View(ViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/trainingcourses/", Name = RouteNames.ApplyApprenticeship.TrainingCourses)]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, TrainingCoursesViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var result = await mediator.Send(new GetTrainingCoursesQuery
            {
                ApplicationId = applicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
            });

            viewModel = new TrainingCoursesViewModel
            {
                ApplicationId = applicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId }),
                TrainingCourses = result.TrainingCourses.Select(t => (TrainingCoursesViewModel.TrainingCourse)t).ToList(),
                ShowTrainingCoursesAchieved = result.TrainingCourses.Any()
            };
            return View(ViewPath, viewModel);
        }

        if (viewModel.ShowTrainingCoursesAchieved)
        {
            var completeSectionCommand = new UpdateTrainingCoursesApplicationCommand
            {
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                ApplicationId = viewModel.ApplicationId,
                TrainingCoursesSectionStatus = viewModel.DoYouWantToAddAnyTrainingCourses.Value ? SectionStatus.InProgress : SectionStatus.Completed
            };

            await mediator.Send(completeSectionCommand);

            return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
        }

        var command = new UpdateTrainingCoursesApplicationCommand
        {
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            ApplicationId = viewModel.ApplicationId,
            TrainingCoursesSectionStatus = viewModel.DoYouWantToAddAnyTrainingCourses.Value ? SectionStatus.InProgress : SectionStatus.Completed
        };

        await mediator.Send(command);

        return viewModel.DoYouWantToAddAnyTrainingCourses.Value
            ? RedirectToRoute(RouteNames.ApplyApprenticeship.AddTrainingCourse, new { viewModel.ApplicationId })
            : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/trainingcourses/add", Name = RouteNames.ApplyApprenticeship.AddTrainingCourse)]
    public IActionResult GetAddATrainingCourse([FromRoute] Guid applicationId)
    {
        var viewModel = new AddTrainingCourseViewModel
        {
            ApplicationId = applicationId
        };

        return View("~/Views/apply/trainingcourses/AddTrainingCourse.cshtml", viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/trainingcourses/add", Name = RouteNames.ApplyApprenticeship.AddTrainingCourse)]
    public async Task<IActionResult> PostAddATrainingCourse(AddTrainingCourseViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/apply/trainingcourses/AddTrainingCourse.cshtml", request);
        }

        var command = new AddTrainingCourseCommand
        {
            ApplicationId = request.ApplicationId,
            CourseName = request.CourseName,
            YearAchieved = int.Parse(request.YearAchieved)
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.ApplyApprenticeship.TrainingCourses, new { request.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/trainingcourses/{trainingCourseId}", Name = RouteNames.ApplyApprenticeship.EditTrainingCourse)]
    public async Task<IActionResult> Edit([FromRoute] Guid applicationId, Guid trainingCourseId)
    {
        var result = await mediator.Send(new GetTrainingCourseQuery
        {
            ApplicationId = applicationId,
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            TrainingCourseId = trainingCourseId
        });

        var viewModel = (EditTrainingCourseViewModel)result;

        return View("~/Views/apply/trainingcourses/EditTrainingCourse.cshtml", viewModel);
    }
}
