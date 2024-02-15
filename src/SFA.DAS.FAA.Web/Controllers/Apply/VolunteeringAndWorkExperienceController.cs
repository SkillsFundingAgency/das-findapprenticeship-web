using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
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
            VolunteeringAndWorkExperienceSectionStatus = model.DoYouWantToAddAnyVolunteeringOrWorkExperience.Value ? SectionStatus.InProgress : SectionStatus.Completed
        };

        await mediator.Send(command);

        return model.DoYouWantToAddAnyVolunteeringOrWorkExperience.Value ? RedirectToRoute("/") : RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/add", Name = RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience)]
    public IActionResult GetAddVolunteeringAndWorkExperience([FromRoute] Guid applicationId)
    {
        var viewModel = new AddVolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId
        };

        return View("~/Views/apply/VolunteeringAndWorkExperience/AddVolunteeringAndWorkExperience.cshtml", viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience/add", Name = RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience)]
    public async Task<IActionResult> PostAddVolunteeringAndWorkExperience(AddVolunteeringAndWorkExperienceViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/apply/VolunteeringAndWorkExperience/AddVolunteeringAndWorkExperience.cshtml", request);
        }

        var command = new AddVolunteeringAndWorkExperienceCommand
        {
            ApplicationId = request.ApplicationId,
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            CompanyName = request.CompanyName,
            Description = request.Description,
            StartDate = request.StartDate.DateTimeValue.Value,
            EndDate = request.IsCurrentRole is true ? null : request.EndDate?.DateTimeValue
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience, new { request.ApplicationId });
    }
}
