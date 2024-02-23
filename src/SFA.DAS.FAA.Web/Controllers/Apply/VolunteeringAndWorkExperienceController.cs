using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

public class VolunteeringAndWorkExperienceController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/volunteeringandworkexperience/List.cshtml";
    private const string AddViewPath = "~/Views/apply/VolunteeringAndWorkExperience/AddVolunteeringAndWorkExperience.cshtml";

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
            VolunteeringAndWorkExperienceSectionStatus = model.DoYouWantToAddAnyVolunteeringOrWorkExperience.Value ? SectionStatus.InProgress : SectionStatus.Completed
        };

        await mediator.Send(command);

        return model.DoYouWantToAddAnyVolunteeringOrWorkExperience.Value 
    }

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/{volunteeringWorkExperienceId}/delete", Name = RouteNames.ApplyApprenticeship.DeleteVolunteeringOrWorkExperience)]
    public async Task<IActionResult> GetDelete([FromRoute] Guid applicationId, Guid volunteeringWorkExperienceId)
    {
        var result = await mediator.Send(new GetVolunteeringOrWorkExperienceItemQuery
        {
            ApplicationId = applicationId,
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            Id = volunteeringWorkExperienceId
        });

        var viewModel = (DeleteVolunteeringOrWorkExperienceViewModel)result;

        return View("~/Views/apply/volunteeringandworkexperience/DeleteVolunteeringOrWorkExperience.cshtml", viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience/{volunteeringWorkExperienceId}/delete", Name = RouteNames.ApplyApprenticeship.DeleteVolunteeringOrWorkExperience)]
    public async Task<IActionResult> PostDelete(DeleteVolunteeringOrWorkExperienceViewModel model)
    {
        try
        {
            var command = new DeleteVolunteeringOrWorkExperienceCommand
            {
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                ApplicationId = model.ApplicationId,
                Id = model.VolunteeringWorkExperienceId
            };
            await mediator.Send(command);
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError(nameof(DeleteVolunteeringOrWorkExperienceViewModel), "There's been a problem");
            return View("~/Views/apply/volunteeringandworkexperience/DeleteVolunteeringOrWorkExperience.cshtml");
        }

        return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience, new { model.ApplicationId });

    }
}
