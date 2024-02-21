using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;


namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class VolunteeringAndWorkExperienceController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/volunteeringandworkexperience/List.cshtml";
    private const string AddViewPath = "~/Views/apply/VolunteeringAndWorkExperience/AddVolunteeringAndWorkExperience.cshtml";
    private const string SummaryViewPath = "~/Views/apply/VolunteeringAndWorkExperience/Summary.cshtml";

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
            ? RedirectToRoute(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience, new { model.ApplicationId }) 
            : RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/add", Name = RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience)]
    public IActionResult GetAddVolunteeringAndWorkExperience([FromRoute] Guid applicationId)
    {
        var viewModel = new AddVolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId
        };

        return View(AddViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience/add", Name = RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience)]
    public async Task<IActionResult> PostAddVolunteeringAndWorkExperience(AddVolunteeringAndWorkExperienceViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(AddViewPath, request);
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

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/summary", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary)]
    public async Task<IActionResult> Summary([FromRoute] Guid applicationId)
    {
        var query = new GetVolunteeringAndWorkExperiencesQuery
        {
            ApplicationId = applicationId,
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
        };

        var result = await mediator.Send(query);

        return View(SummaryViewPath, new VolunteeringAndWorkExperienceSummaryViewModel
        {
            ApplicationId = applicationId,
            ChangeLinkUrl = string.Empty, //TODO: Redirect the user to the Edit Page
            DeleteLinkUrl = string.Empty, //TODO: Redirect the user to the Delete Page
            AddAnotherVolunteeringAndWorkExperienceLinkUrl = Url.RouteUrl(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience, new { applicationId }),
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId }),
            WorkHistories = result.VolunteeringAndWorkExperiences.Select(wk => (WorkHistoryViewModel)wk).ToList(),
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience/summary", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary)]
    public async Task<IActionResult> Summary(VolunteeringAndWorkExperienceSummaryViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var result = await mediator.Send(new GetVolunteeringAndWorkExperiencesQuery
            {
                ApplicationId = viewModel.ApplicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
            });

            viewModel = new VolunteeringAndWorkExperienceSummaryViewModel
            {
                ApplicationId = viewModel.ApplicationId,
                ChangeLinkUrl = string.Empty, //TODO: Redirect the user to the Edit Page
                DeleteLinkUrl = string.Empty, //TODO: Redirect the user to the Delete Page
                AddAnotherVolunteeringAndWorkExperienceLinkUrl = Url.RouteUrl(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience, new { viewModel.ApplicationId }),
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId }),
                WorkHistories = result.VolunteeringAndWorkExperiences.Select(wk => (WorkHistoryViewModel)wk).ToList(),
            };
            return View(SummaryViewPath, viewModel);
        }

        var command = new UpdateVolunteeringAndWorkExperienceApplicationCommand
        {
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            ApplicationId = viewModel.ApplicationId,
            VolunteeringAndWorkExperienceSectionStatus = viewModel.IsSectionCompleted.HasValue && viewModel.IsSectionCompleted.Value
                ? SectionStatus.Completed 
                : SectionStatus.InProgress
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }
}
