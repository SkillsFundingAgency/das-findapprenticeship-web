using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.UpdateVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Commands.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using System;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class VolunteeringAndWorkExperienceController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/volunteeringandworkexperience/List.cshtml";
    private const string AddViewPath = "~/Views/apply/VolunteeringAndWorkExperience/AddVolunteeringAndWorkExperience.cshtml";
    private const string EditViewPath = "~/Views/apply/volunteeringandworkexperience/EditVolunteeringAndWorkExperience.cshtml";
    private const string SummaryViewPath = "~/Views/apply/VolunteeringAndWorkExperience/Summary.cshtml";
    private const string DeleteViewPath = "~/Views/apply/volunteeringandworkexperience/DeleteVolunteeringOrWorkExperience.cshtml";


    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience)]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId)
    {
        var result = await mediator.Send(new GetVolunteeringAndWorkExperiencesQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId()
        });

        if (result.VolunteeringAndWorkExperiences.Count > 0)
        {
            return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary, new { applicationId });
        }

        return View(ViewPath, new VolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId }),
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience)]
    public async Task<IActionResult> Post(VolunteeringAndWorkExperienceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model = new VolunteeringAndWorkExperienceViewModel
            {
                ApplicationId = model.ApplicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { model.ApplicationId }),
            };
            return View(ViewPath, model);
        }

        var command = new UpdateVolunteeringAndWorkExperienceApplicationCommand
        {
            CandidateId = User.Claims.CandidateId(),
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
            CandidateId = User.Claims.CandidateId(),
            CompanyName = request.CompanyName,
            Description = request.Description,
            StartDate = request.StartDate.DateTimeValue.Value,
            EndDate = request.IsCurrentRole is true ? null : request.EndDate?.DateTimeValue
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary, new { request.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/summary", Name = RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary)]
    public async Task<IActionResult> Summary([FromRoute] Guid applicationId)
    {
        var query = new GetVolunteeringAndWorkExperiencesQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId()
        };

        var result = await mediator.Send(query);

        if (result.VolunteeringAndWorkExperiences.Count > 0)
        {
            return View(SummaryViewPath, new VolunteeringAndWorkExperienceSummaryViewModel
            {
                ApplicationId = applicationId,
                AddAnotherVolunteeringAndWorkExperienceLinkUrl = Url.RouteUrl(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience, new { applicationId }),
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId }),
                WorkHistories = result.VolunteeringAndWorkExperiences.Select(wk => (WorkHistoryViewModel)wk).ToList(),
            });
        }

        return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience, new { applicationId });
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
                CandidateId = User.Claims.CandidateId()
            });

            viewModel = new VolunteeringAndWorkExperienceSummaryViewModel
            {
                ApplicationId = viewModel.ApplicationId,
                AddAnotherVolunteeringAndWorkExperienceLinkUrl = Url.RouteUrl(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience, new { viewModel.ApplicationId }),
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId }),
                WorkHistories = result.VolunteeringAndWorkExperiences.Select(wk => (WorkHistoryViewModel)wk).ToList(),
            };
            return View(SummaryViewPath, viewModel);
        }

        var command = new UpdateVolunteeringAndWorkExperienceApplicationCommand
        {
            CandidateId = User.Claims.CandidateId(),
            ApplicationId = viewModel.ApplicationId,
            VolunteeringAndWorkExperienceSectionStatus = viewModel.IsSectionCompleted.HasValue && viewModel.IsSectionCompleted.Value
                ? SectionStatus.Completed 
                : SectionStatus.InProgress
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/{volunteeringWorkExperienceId}", Name = RouteNames.ApplyApprenticeship.EditVolunteeringAndWorkExperience)]
    public async Task<IActionResult> GetEditVolunteeringAndWorkExperience([FromRoute] Guid applicationId, Guid volunteeringWorkExperienceId)
    {
        var result = await mediator.Send(new GetVolunteeringOrWorkExperienceItemQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId(),
            Id = volunteeringWorkExperienceId
        });

        var viewModel = (EditVolunteeringAndWorkExperienceViewModel)result;

        return View(EditViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience/{volunteeringWorkExperienceId}", Name = RouteNames.ApplyApprenticeship.EditVolunteeringAndWorkExperience)]
    public async Task<IActionResult> PostEditVolunteeringAndWorkExperience(EditVolunteeringAndWorkExperienceViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(EditViewPath, request);
        }

        var command = new UpdateVolunteeringAndWorkExperienceCommand
        {
            VolunteeringOrWorkExperienceId = request.VolunteeringOrWorkExperienceId,
            ApplicationId = request.ApplicationId,
            CandidateId = User.Claims.CandidateId(),
            CompanyName = request.CompanyName,
            Description = request.Description,
            StartDate = request.StartDate.DateTimeValue.Value,
            EndDate = request.IsCurrentRole is true ? null : request.EndDate?.DateTimeValue
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary, new { request.ApplicationId });
    }

    [HttpGet]
    [Route("apply/{applicationId}/volunteering-and-work-experience/{volunteeringWorkExperienceId}/delete", Name = RouteNames.ApplyApprenticeship.DeleteVolunteeringOrWorkExperience)]
    public async Task<IActionResult> GetDelete([FromRoute] Guid applicationId, Guid volunteeringWorkExperienceId)
    {
        var result = await mediator.Send(new GetVolunteeringOrWorkExperienceItemQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId(),
            Id = volunteeringWorkExperienceId
        });

        var viewModel = (DeleteVolunteeringOrWorkExperienceViewModel)result;

        return View(DeleteViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/volunteering-and-work-experience/{volunteeringWorkExperienceId}/delete", Name = RouteNames.ApplyApprenticeship.DeleteVolunteeringOrWorkExperience)]
    public async Task<IActionResult> PostDelete(DeleteVolunteeringOrWorkExperienceViewModel model)
    {
        try
        {
            var command = new DeleteVolunteeringOrWorkExperienceCommand
            {
                CandidateId = User.Claims.CandidateId(),
                ApplicationId = model.ApplicationId,
                Id = model.VolunteeringWorkExperienceId
            };
            await mediator.Send(command);
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError(nameof(DeleteVolunteeringOrWorkExperienceViewModel), "There's been a problem");
            return View(DeleteViewPath);
        }

        return RedirectToRoute(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary, new { model.ApplicationId });
    }
}
