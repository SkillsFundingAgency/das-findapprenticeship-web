using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class SkillsAndStrengthsController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/skillsandstrengths/List.cshtml";

    [HttpGet]
    [Route("apply/{applicationId}/skillsandstrengths/", Name = RouteNames.ApplyApprenticeship.SkillsAndStrengths)]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId)
    {
        var expectedResult = await mediator.Send(new GetExpectedSkillsAndStrengthsQuery
        {
            ApplicationId = applicationId,
            CandidateId = (Guid)User.Claims.CandidateId()!
        });

        var viewModel = new SkillsAndStrengthsViewModel(expectedResult, applicationId);
        return View(ViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId}/skillsandstrengths/", Name = RouteNames.ApplyApprenticeship.SkillsAndStrengths)]
    public async Task<IActionResult> Post(
        [FromServices] IValidator<SkillsAndStrengthsViewModel> validator,
        [FromRoute] Guid applicationId,
        SkillsAndStrengthsViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        if (!ModelState.IsValid)
        {
            if (viewModel.AutoSave)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new JsonResult(null);
            }
            
            var expectedResult = await mediator.Send(new GetExpectedSkillsAndStrengthsQuery
            {
                ApplicationId = applicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            viewModel = new SkillsAndStrengthsViewModel(expectedResult, applicationId);
            return View(ViewPath, viewModel);
        }

        var updateCommand = new UpdateSkillsAndStrengthsCommand
        {
            CandidateId = (Guid)User.Claims.CandidateId()!,
            ApplicationId = viewModel.ApplicationId,
            SkillsAndStrengths = viewModel.SkillsAndStrengths,
            SkillsAndStrengthsSectionStatus = viewModel.IsSectionComplete is true ? SectionStatus.Completed : SectionStatus.Incomplete
        };

        await mediator.Send(updateCommand);
        
        return viewModel.AutoSave
            ? new JsonResult(StatusCodes.Status200OK)
            : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }
}
