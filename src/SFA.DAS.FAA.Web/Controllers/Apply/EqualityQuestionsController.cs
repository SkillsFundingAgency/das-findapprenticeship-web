using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.EqualityQuestions;
using SFA.DAS.FAA.Application.Queries.EqualityQuestions;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;
using static SFA.DAS.FAA.Web.Infrastructure.RouteNames.ApplyApprenticeship;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
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
        [Route("gender", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender)]
        public async Task<IActionResult> Gender([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(GenderQuestionsViewPath, (EqualityQuestionsGenderViewModel)equalityQuestions)
                : View(GenderQuestionsViewPath, new EqualityQuestionsGenderViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("gender", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender)]
        public async Task<IActionResult> Gender([FromQuery] Guid? applicationId, EqualityQuestionsGenderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(GenderQuestionsViewPath, viewModel);
            }

            var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
            var cacheItem = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
            if (cacheItem == null) return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender, new { applicationId });
            cacheItem.Apply(viewModel);
            await cacheStorageService.Set(cacheKey, cacheItem);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup, new { applicationId });
        }

        [HttpGet]
        [Route("ethnic-group", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup)]
        public async Task<IActionResult> EthnicGroup([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicGroupQuestionsViewPath, (EqualityQuestionsEthnicGroupViewModel)equalityQuestions)
                : View(EthnicGroupQuestionsViewPath, new EqualityQuestionsEthnicGroupViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup)]
        public async Task<IActionResult> EthnicGroup([FromQuery] Guid? applicationId, EqualityQuestionsEthnicGroupViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(EthnicGroupQuestionsViewPath, viewModel);
            }

            var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
            var cacheItem = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
            if (cacheItem == null) return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender, new { applicationId });
            cacheItem.Apply(viewModel);
            await cacheStorageService.Set(cacheKey, cacheItem);

            return RedirectToRoute(RouteNamesHelperService.GetEqualityFlowEthnicSubGroupRoute(cacheItem.EthnicGroup), new { applicationId });
        }

        [HttpGet]
        [Route("ethnic-group/white", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public async Task<IActionResult> EthnicGroupWhite([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupWhiteQuestionsViewPath, (EqualityQuestionsEthnicSubGroupWhiteViewModel)equalityQuestions)
                : View(EthnicSubGroupWhiteQuestionsViewPath, new EqualityQuestionsEthnicSubGroupWhiteViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/white", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public async Task<IActionResult> EthnicGroupWhite([FromQuery] Guid? applicationId, EqualityQuestionsEthnicSubGroupWhiteViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupWhiteQuestionsViewPath, viewModel)
                : await UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/mixed", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
        public async Task<IActionResult> EthnicGroupMixed([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupMixedQuestionsViewPath, (EqualityQuestionsEthnicSubGroupMixedViewModel)equalityQuestions)
                : View(EthnicSubGroupMixedQuestionsViewPath, new EqualityQuestionsEthnicSubGroupMixedViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/mixed", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
        public async Task<IActionResult> EthnicGroupMixed([FromQuery] Guid? applicationId, EqualityQuestionsEthnicSubGroupMixedViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupMixedQuestionsViewPath, viewModel)
                : await UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/asian", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
        public async Task<IActionResult> EthnicGroupAsian([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupAsianQuestionsViewPath, (EqualityQuestionsEthnicSubGroupAsianViewModel)equalityQuestions)
                : View(EthnicSubGroupAsianQuestionsViewPath, new EqualityQuestionsEthnicSubGroupAsianViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/asian", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
        public async Task<IActionResult> EthnicGroupAsian([FromQuery] Guid? applicationId, EqualityQuestionsEthnicSubGroupAsianViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupAsianQuestionsViewPath, viewModel)
                : await UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/black", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
        public async Task<IActionResult> EthnicGroupBlack([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupBlackQuestionsViewPath, (EqualityQuestionsEthnicSubGroupBlackViewModel)equalityQuestions)
                : View(EthnicSubGroupBlackQuestionsViewPath, new EqualityQuestionsEthnicSubGroupBlackViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/black", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
        public async Task<IActionResult> EthnicGroupBlack([FromQuery] Guid? applicationId, EqualityQuestionsEthnicSubGroupBlackViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupBlackQuestionsViewPath, viewModel)
                : await UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/other", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
        public async Task<IActionResult> EthnicGroupOther([FromQuery] Guid? applicationId)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupOtherQuestionsViewPath, (EqualityQuestionsEthnicSubGroupOtherViewModel)equalityQuestions)
                : View(EthnicSubGroupOtherQuestionsViewPath, new EqualityQuestionsEthnicSubGroupOtherViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/other", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
        public async Task<IActionResult> EthnicGroupOther([FromQuery] Guid? applicationId, EqualityQuestionsEthnicSubGroupOtherViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupOtherQuestionsViewPath, viewModel)
                : await UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
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

            if (equalityQuestions is null)
            {
                return RedirectToRoute(EqualityQuestions.EqualityFlowGender,
                    new { applicationId });
            }

            return View(SummaryViewPath, (EqualityQuestionsSummaryViewModel)equalityQuestions);
        }

        [HttpPost]
        [Route("summary", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary)]
        public async Task<IActionResult> Summary([FromQuery] Guid? applicationId, EqualityQuestionsSummaryViewModel viewModel)
        {
            var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
            var equalityQuestions = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

            if (equalityQuestions is null)
                return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender,
                    new { applicationId });

            await mediator.Send(new CreateEqualityQuestionsCommand
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                EthnicGroup = equalityQuestions.EthnicGroup,
                EthnicSubGroup = equalityQuestions.EthnicSubGroup,
                Sex = equalityQuestions.Sex,
                IsGenderIdentifySameSexAtBirth = equalityQuestions.IsGenderIdentifySameSexAtBirth,
                OtherEthnicSubGroupAnswer = equalityQuestions.OtherEthnicSubGroupAnswer,
            });

            if (applicationId == null)
            {
                return RedirectToRoute(RouteNames.Settings);
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.ApplicationSubmittedConfirmation,
                new { applicationId });
        }

        private async Task<EqualityQuestionsModel?> GetEqualityQuestionsFromCacheMemory()
        {
            var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
            var equalityQuestions = await cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
            return equalityQuestions ?? null;
        }

        private async Task<RedirectToRouteResult> UpdateEqualityQuestionModel(Guid? applicationId, string? subGroup, string? subGroupAnswer)
        {
            var equalityQuestions = await GetEqualityQuestionsFromCacheMemory();

            if (equalityQuestions is null)
                return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender,
                    new { applicationId });

            var ethnicSubGroup = (EthnicSubGroup)Enum.Parse(typeof(EthnicSubGroup), subGroup!, true);

            equalityQuestions.EthnicSubGroup = ethnicSubGroup;
            equalityQuestions.OtherEthnicSubGroupAnswer = ethnicSubGroup is EthnicSubGroup.AnyOtherWhiteBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherAsianBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherBlackAfricanOrCaribbeanBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherMixedBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherEthnicGroup
                ? subGroupAnswer
                : string.Empty;

            var cacheKey = string.Format($"{Key}", User.Claims.CandidateId());
            await cacheStorageService.Set(cacheKey, equalityQuestions);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary, new { applicationId });
        }
    }
}