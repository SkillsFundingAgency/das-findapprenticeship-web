using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Enums;
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
        return View(ViewPath, new VolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience)]
    public async Task<IActionResult> Post(VolunteeringAndWorkExperienceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { model.ApplicationId });
            return View(ViewPath, model);
        }

        var command = new UpdateVolunteeringAndWorkExperienceApplicationCommand
        {
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            ApplicationId = model.ApplicationId,
            VolunteeringAndWorkExperienceSectionStatus = model.DoYouWantToAddAnyVolunteeringAndWorkExperience.Value ? SectionStatus.InProgress : SectionStatus.Completed
        };

        await mediator.Send(command);

        return model.DoYouWantToAddAnyVolunteeringAndWorkExperience.Value ? RedirectToRoute("/") : RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
    }
}
