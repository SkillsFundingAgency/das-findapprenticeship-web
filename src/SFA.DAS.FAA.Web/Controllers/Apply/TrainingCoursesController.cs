using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

public class TrainingCoursesController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/trainingcourses/List.cshtml";

    [HttpGet]
    [Route("apply/{applicationId}/trainingcourses/", Name = RouteNames.ApplyApprenticeship.TrainingCourses)]
    public IActionResult Get([FromRoute] Guid applicationId)
    {
        return View(ViewPath, new TrainingCoursesViewModel
        {
            ApplicationId = applicationId,
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/trainingcourses/", Name = RouteNames.ApplyApprenticeship.TrainingCourses)]
    public async Task<IActionResult> Post(TrainingCoursesViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId });
            return View(ViewPath, viewModel);
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

        return RedirectToRoute("/", new { request.ApplicationId });
    }

}
