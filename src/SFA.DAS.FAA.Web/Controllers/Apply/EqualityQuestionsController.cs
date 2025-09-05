using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.EqualityQuestions;
using SFA.DAS.FAA.Application.Queries.EqualityQuestions;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Models.Apply.Base;
using SFA.DAS.FAA.Web.Services;
using static SFA.DAS.FAA.Web.Infrastructure.RouteNames.ApplyApprenticeship;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
[Route("equality-questions")]
public class EqualityQuestionsController(IMediator mediator, ICacheStorageService cacheStorageService) : Controller
{
    private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";

    private const string GenderQuestionsViewPath = "~/Views/apply/EqualityQuestions/Gender.cshtml";
    private const string EthnicGroupQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicGroup.cshtml";
    private const string EthnicSubGroupWhiteQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicSubGroupWhite.cshtml";
    private const string EthnicSubGroupMixedQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicSubGroupMixed.cshtml";
    private const string EthnicSubGroupAsianQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicSubGroupAsian.cshtml";
    private const string EthnicSubGroupBlackQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicSubGroupBlack.cshtml";
    private const string EthnicSubGroupOtherQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicSubGroupOther.cshtml";
    private const string SummaryViewPath = "~/Views/apply/EqualityQuestions/Summary.cshtml";

    [HttpGet]
    [Route("gender", Name = EqualityQuestions.EqualityFlowGender)]
    public async Task<IActionResult> Gender([FromQuery] Guid? applicationId, bool isEdit = false)
    {
        var cacheItem = await GetEqualityQuestionsFromCacheMemory();

        if (cacheItem == null)
        {
            var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
            cacheItem = new EqualityQuestionsModel{ ApplicationId = applicationId };
            await cacheStorageService.Set(cacheKey, cacheItem);
        }

        var viewModel = (EqualityQuestionsGenderViewModel)cacheItem;
        viewModel.IsEdit = isEdit;

        return View(GenderQuestionsViewPath, (EqualityQuestionsGenderViewModel)cacheItem);

    }

    [HttpPost]
    [Route("gender", Name = EqualityQuestions.EqualityFlowGender)]
    public async Task<IActionResult> Gender([FromQuery] Guid? applicationId, EqualityQuestionsGenderViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(GenderQuestionsViewPath, viewModel);
        }

        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        var cacheItem = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
        if (cacheItem is null) return RedirectToStart(applicationId);
        cacheItem.Apply(viewModel);
        await cacheStorageService.Set(cacheKey, cacheItem);

        if (viewModel.IsEdit)
        {
            return RedirectToRoute(EqualityQuestions.EqualityFlowSummary, new { applicationId });
        }

        return RedirectToRoute(EqualityQuestions.EqualityFlowEthnicGroup, new { applicationId });
    }

    [HttpGet]
    [Route("ethnic-group", Name = EqualityQuestions.EqualityFlowEthnicGroup)]
    public async Task<IActionResult> EthnicGroup([FromQuery] Guid? applicationId, bool isEdit = false, bool clear = false)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
        if (equalityQuestions is null) return RedirectToStart(applicationId);

        if (clear) { equalityQuestions.SelectedEthnicGroup = null; }

        var viewModel = (EqualityQuestionsEthnicGroupViewModel)equalityQuestions;
        viewModel.IsEdit = isEdit;

        return View(EthnicGroupQuestionsViewPath, viewModel);
    }

    [HttpPost]
    [Route("ethnic-group", Name = EqualityQuestions.EqualityFlowEthnicGroup)]
    public async Task<IActionResult> EthnicGroup(
        [FromServices] IValidator<EqualityQuestionsEthnicGroupViewModel> validator,
        [FromQuery] Guid? applicationId,
        EqualityQuestionsEthnicGroupViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        if (!ModelState.IsValid)
        {
            return View(EthnicGroupQuestionsViewPath, viewModel);
        }

        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        var cacheItem = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
        if (cacheItem is null) return RedirectToStart(applicationId);
        cacheItem.Apply(viewModel);
        await cacheStorageService.Set(cacheKey, cacheItem);

        var selectedOption = (EthnicGroup)Enum.Parse(typeof(EthnicGroup), viewModel.EthnicGroup!, true);

        if (selectedOption == Domain.Enums.EthnicGroup.PreferNotToSay)
        {
            await UpdateEqualityQuestionModel(applicationId, (EqualityQuestionsEthnicSubGroupPreferNotToSaveViewModel)viewModel );
        }
            
        return RedirectToRoute(RouteNamesHelperService.GetEqualityFlowEthnicSubGroupRoute(selectedOption), new { applicationId, viewModel.IsEdit });
    }

    [HttpGet]
    [Route("ethnic-group/white", Name = EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
    public async Task<IActionResult> EthnicGroupWhite([FromQuery] Guid? applicationId, bool isEdit = false)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
        if (equalityQuestions is null) return RedirectToStart(applicationId);
            
        var viewModel = (EqualityQuestionsEthnicSubGroupWhiteViewModel)equalityQuestions;
        viewModel.IsEdit = isEdit;
        return View(EthnicSubGroupWhiteQuestionsViewPath, viewModel);
    }

    [HttpPost]
    [Route("ethnic-group/white", Name = EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
    public async Task<IActionResult> EthnicGroupWhite(
        [FromServices] IValidator<EqualityQuestionsEthnicSubGroupWhiteViewModel> validator,
        [FromQuery] Guid? applicationId,
        EqualityQuestionsEthnicSubGroupWhiteViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        return !ModelState.IsValid
            ? View(EthnicSubGroupWhiteQuestionsViewPath, viewModel)
            : await UpdateEqualityQuestionModel(applicationId, viewModel);
    }

    [HttpGet]
    [Route("ethnic-group/mixed", Name = EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
    public async Task<IActionResult> EthnicGroupMixed([FromQuery] Guid? applicationId, bool isEdit = false)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
        if (equalityQuestions is null) return RedirectToStart(applicationId);

        var viewModel = (EqualityQuestionsEthnicSubGroupMixedViewModel)equalityQuestions;
        viewModel.IsEdit = isEdit;

        return View(EthnicSubGroupMixedQuestionsViewPath, viewModel);
    }

    [HttpPost]
    [Route("ethnic-group/mixed", Name = EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
    public async Task<IActionResult> EthnicGroupMixed(
        [FromServices] IValidator<EqualityQuestionsEthnicSubGroupMixedViewModel> validator,
        [FromQuery] Guid? applicationId, 
        EqualityQuestionsEthnicSubGroupMixedViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        return !ModelState.IsValid
            ? View(EthnicSubGroupMixedQuestionsViewPath, viewModel)
            : await UpdateEqualityQuestionModel(applicationId, viewModel);
    }

    [HttpGet]
    [Route("ethnic-group/asian", Name = EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
    public async Task<IActionResult> EthnicGroupAsian([FromQuery] Guid? applicationId, bool isEdit = false)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
        if (equalityQuestions is null) return RedirectToStart(applicationId);

        var viewModel = (EqualityQuestionsEthnicSubGroupAsianViewModel) equalityQuestions;
        viewModel.IsEdit = isEdit;

        return View(EthnicSubGroupAsianQuestionsViewPath, viewModel);
    }

    [HttpPost]
    [Route("ethnic-group/asian", Name = EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
    public async Task<IActionResult> EthnicGroupAsian(
        [FromServices] IValidator<EqualityQuestionsEthnicSubGroupAsianViewModel> validator,
        [FromQuery] Guid? applicationId,
        EqualityQuestionsEthnicSubGroupAsianViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        return !ModelState.IsValid
            ? View(EthnicSubGroupAsianQuestionsViewPath, viewModel)
            : await UpdateEqualityQuestionModel(applicationId, viewModel);
    }

    [HttpGet]
    [Route("ethnic-group/black", Name = EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
    public async Task<IActionResult> EthnicGroupBlack([FromQuery] Guid? applicationId, bool isEdit = false)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
        if (equalityQuestions is null) return RedirectToStart(applicationId);

        var viewModel = (EqualityQuestionsEthnicSubGroupBlackViewModel)equalityQuestions;
        viewModel.IsEdit = isEdit;

        return View(EthnicSubGroupBlackQuestionsViewPath, viewModel);
    }

    [HttpPost]
    [Route("ethnic-group/black", Name = EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
    public async Task<IActionResult> EthnicGroupBlack(
        [FromServices] IValidator<EqualityQuestionsEthnicSubGroupBlackViewModel> validator,
        [FromQuery] Guid? applicationId,
        EqualityQuestionsEthnicSubGroupBlackViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        return !ModelState.IsValid
            ? View(EthnicSubGroupBlackQuestionsViewPath, viewModel)
            : await UpdateEqualityQuestionModel(applicationId, viewModel);
    }

    [HttpGet]
    [Route("ethnic-group/other", Name = EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
    public async Task<IActionResult> EthnicGroupOther([FromQuery] Guid? applicationId, bool isEdit = false)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
        if (equalityQuestions is null) return RedirectToStart(applicationId);

        var viewModel = (EqualityQuestionsEthnicSubGroupOtherViewModel)equalityQuestions;
        viewModel.IsEdit = isEdit;

        return View(EthnicSubGroupOtherQuestionsViewPath, viewModel);
    }

    [HttpPost]
    [Route("ethnic-group/other", Name = EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
    public async Task<IActionResult> EthnicGroupOther(
        [FromServices] IValidator<EqualityQuestionsEthnicSubGroupOtherViewModel> validator,
        [FromQuery] Guid? applicationId,
        EqualityQuestionsEthnicSubGroupOtherViewModel viewModel)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        return !ModelState.IsValid
            ? View(EthnicSubGroupOtherQuestionsViewPath, viewModel)
            : await UpdateEqualityQuestionModel(applicationId, viewModel);
    }

    [HttpGet]
    [Route("edit", Name = EqualityQuestions.EditEqualityQuestions)]
    public async Task<IActionResult> Edit()
    {
        var candidateId = User.Claims.CandidateId();
        var queryResult = await mediator.Send(new GetEqualityQuestionsQuery
        {
            CandidateId = candidateId ?? Guid.Empty
        });

        var equalityQuestions = EqualityQuestionsModel.MapFromQueryResult(queryResult);

        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        await cacheStorageService.Set(cacheKey, equalityQuestions);

        return RedirectToRoute(EqualityQuestions.EqualityFlowSummary);
    }

    [HttpGet]
    [Route("summary", Name = EqualityQuestions.EqualityFlowSummary)]
    public async Task<IActionResult> Summary([FromQuery] Guid? applicationId)
    {
        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        var equalityQuestions = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

        if (equalityQuestions is null) return RedirectToStart(applicationId);

        return View(SummaryViewPath, (EqualityQuestionsSummaryViewModel)equalityQuestions);
    }

    [HttpPost]
    [Route("summary", Name = EqualityQuestions.EqualityFlowSummary)]
    public async Task<IActionResult> Summary([FromQuery] Guid? applicationId, EqualityQuestionsSummaryViewModel viewModel)
    {
        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        var equalityQuestions = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

        if (equalityQuestions is null) return RedirectToStart(applicationId);
            
        await mediator.Send(new CreateEqualityQuestionsCommand
        {
            CandidateId = (Guid)User.Claims.CandidateId()!,
            EthnicGroup = equalityQuestions.EthnicGroup,
            EthnicSubGroup = equalityQuestions.EthnicSubGroup,
            Sex = equalityQuestions.Sex,
            IsGenderIdentifySameSexAtBirth = equalityQuestions.IsGenderIdentifySameSexAtBirth,
            OtherEthnicSubGroupAnswer = equalityQuestions.OtherEthnicSubGroupAnswer,
        });

        await cacheStorageService.Remove(cacheKey);

        return applicationId == null 
            ? RedirectToRoute(RouteNames.Settings) 
            : RedirectToRoute(RouteNames.Applications.ViewApplications, new { tab = ApplicationsTab.Submitted, showEqualityQuestionsBanner = true });
    }

    private async Task<EqualityQuestionsModel?> GetEqualityQuestionsFromCacheMemory()
    {
        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        var equalityQuestions = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
        return equalityQuestions;
    }

    private async Task<RedirectToRouteResult> UpdateEqualityQuestionModel(Guid? applicationId, EqualityQuestionEthnicSubGroupViewModelBase source)
    {
        var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();

        if (equalityQuestions is null) return RedirectToStart(applicationId);

        equalityQuestions.Apply(source);

        var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
        await cacheStorageService.Set(cacheKey, equalityQuestions);

        return RedirectToRoute(EqualityQuestions.EqualityFlowSummary, new { applicationId });
    }

    private RedirectToRouteResult RedirectToStart(Guid? applicationId)
    {
        return applicationId.HasValue
            ? RedirectToRoute(EqualityQuestions.EqualityFlowGender, new { applicationId })
            : RedirectToRoute(EqualityQuestions.EditEqualityQuestions);
    }
}