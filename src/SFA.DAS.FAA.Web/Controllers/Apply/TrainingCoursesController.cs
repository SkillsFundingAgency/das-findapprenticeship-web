﻿using MediatR;
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
        return View(ViewPath, new AddTrainingCourseViewModel
        {
            ApplicationId = applicationId,
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/trainingcourses/", Name = RouteNames.ApplyApprenticeship.TrainingCourses)]
    public async Task<IActionResult> Post(AddTrainingCourseViewModel viewModel)
    {
        if (string.IsNullOrEmpty(viewModel.AddTrainingCourse))
        {
            ModelState.AddModelError(nameof(viewModel.AddTrainingCourse), "Select if you want to add any training courses");
            viewModel.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId });
            return View(ViewPath, viewModel);
        }

        var command = new UpdateTrainingCoursesApplicationCommand
        {
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            ApplicationId = viewModel.ApplicationId,
            TrainingCoursesSectionStatus = viewModel.AddTrainingCourse is "Yes" ? SectionStatus.InProgress : SectionStatus.Completed
        };

        await mediator.Send(command);

        return viewModel.AddTrainingCourse.Equals("Yes")
            ? RedirectToRoute("/") //TODO: Redirect the user to Add Training Course Page.
            : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }
}