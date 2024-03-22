using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;
using SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;
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
    [Route("apply/{applicationId}/additional-question/{additionalQuestion}/{additionalQuestionId}", Name = RouteNames.ApplyApprenticeship.AddAdditionalQuestion)]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromRoute] int additionalQuestion, [FromRoute] Guid additionalQuestionId)
    {
        var result = await mediator.Send(new GetAdditionalQuestionQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId(),
            AdditionalQuestionId = additionalQuestionId,
            AdditionalQuestion = additionalQuestion,
        });

        return View(AddViewPath, new AddAdditionalQuestionViewModel
        {
            ApplicationId = applicationId,
            AdditionalQuestionLabel = result.QuestionText,
            AdditionalQuestionAnswer = result.Answer,
            IsSectionCompleted = result.IsSectionCompleted
        });
    }

    [HttpPost]
    [Route("apply/{applicationId}/additional-question/{additionalQuestion}/{additionalQuestionId}", Name = RouteNames.ApplyApprenticeship.AddAdditionalQuestion)]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromRoute] int additionalQuestion, [FromRoute] Guid additionalQuestionId, AddAdditionalQuestionViewModel viewModel)
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
            UpdatedAdditionalQuestion = additionalQuestion,
            AdditionalQuestionSectionStatus = viewModel.IsSectionCompleted != null && viewModel.IsSectionCompleted.Value ? SectionStatus.Completed : SectionStatus.Incomplete
        };

        await mediator.Send(command);

        return RedirectToRoute(RouteNames.Apply, new { applicationId });
    }
}