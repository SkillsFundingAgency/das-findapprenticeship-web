using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.AdditionalQuestion;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class AdditionalQuestionController(IMediator mediator) : Controller
{
    private const string AddViewPath = "~/Views/apply/AdditionalQuestion/AddAdditionalQuestion.cshtml";

    [HttpGet]
    [Route("apply/{applicationId}/additional-question/{additionalQuestionId}", Name = RouteNames.ApplyApprenticeship.AddAdditionalQuestion)]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromRoute] Guid additionalQuestionId)
    {
        var result = await mediator.Send(new GetAdditionalQuestionQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId(),
            AdditionQuestionId = additionalQuestionId
        });

        return View(AddViewPath, new AddAdditionalQuestionViewModel
        {
            ApplicationId = applicationId,
            AdditionalQuestionLabel = result.QuestionText
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/additional-question/{additionalQuestionId}", Name = RouteNames.ApplyApprenticeship.AddAdditionalQuestion)]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromRoute] Guid additionalQuestionId, AddAdditionalQuestionViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(AddViewPath, viewModel);
        }

        var command = new AddAdditionalQuestionCommand
        {
            CandidateId = User.Claims.CandidateId(),
            ApplicationId = applicationId,
            Id = additionalQuestionId,
            Answer = viewModel.AdditionalQuestionAnswer,
        };

        await mediator.Send(command);

        var completeSectionCommand = new UpdateAdditionalQuestionApplicationCommand
        {
            CandidateId = User.Claims.CandidateId(),
            ApplicationId = viewModel.ApplicationId,
            AdditionQuestionOne = viewModel.IsSectionCompleted.Value ? SectionStatus.Completed : SectionStatus.InProgress
        };

        await mediator.Send(completeSectionCommand);

        return RedirectToRoute(RouteNames.Apply, new { applicationId });
    }
}