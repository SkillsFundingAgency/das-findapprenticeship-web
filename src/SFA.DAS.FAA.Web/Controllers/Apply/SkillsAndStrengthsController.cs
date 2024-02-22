using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.SkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmployerSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;
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
        var result = await mediator.Send(new GetSkillsAndStrengthsQuery
        {
            ApplicationId = applicationId,
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
        });

        var viewModel = (SkillsAndStrengthsViewModel)result;
        return View(ViewPath, viewModel);
    }
    [HttpPost]
    [Route("apply/{applicationId}/skillsandstrengths/", Name = RouteNames.ApplyApprenticeship.SkillsAndStrengths)]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, SkillsAndStrengthsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var result = await mediator.Send(new GetSkillsAndStrengthsQuery
            {
                ApplicationId = applicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
            });

            viewModel = (SkillsAndStrengthsViewModel)result;
            return View(ViewPath, viewModel);
        }

        var command = new UpdateSkillsAndStrengthsApplicationCommand
        {
            CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
            ApplicationId = viewModel.ApplicationId,
            SkillsAndStrengthsSectionStatus = viewModel.IsSectionComplete.Value ? SectionStatus.Completed : SectionStatus.InProgress
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }
}
