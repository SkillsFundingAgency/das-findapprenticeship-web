using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

public class VolunteeringAndWorkExperienceController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/volunteeringandworkexperience/List.cshtml";

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience)]
    public IActionResult Get([FromRoute] Guid applicationId)
    {
        return View(ViewPath, new AddVolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience)]
    public async Task<IActionResult> Post(AddVolunteeringAndWorkExperienceViewModel model)
    {
        if (string.IsNullOrEmpty(model.AddVolunteeringAndWorkExperience))
        {
            ModelState.AddModelError(nameof(model.AddVolunteeringAndWorkExperience), "Select if you want to add any volunteering or work experience");
            model.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { model.ApplicationId });
            return View(ViewPath, model);
        }

        var command = new UpdateVolunteeringAndWorkExperienceApplicationCommand
        {
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            ApplicationId = model.ApplicationId,
            VolunteeringAndWorkExperienceSectionStatus = model.AddVolunteeringAndWorkExperience is "Yes" ? Domain.Enums.SectionStatus.InProgress : Domain.Enums.SectionStatus.Completed
        };

        await mediator.Send(command);

        return model.AddVolunteeringAndWorkExperience.Equals("Yes") ? RedirectToRoute("/") : RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
    }
}
