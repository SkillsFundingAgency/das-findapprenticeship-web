using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("apply/{applicationId}/equality-questions")]
    public class EqualityQuestionsController(ICacheStorageService cacheStorageService) : Controller
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
        public IActionResult Gender([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(GenderQuestionsViewPath, (EqualityQuestionsGenderViewModel)equalityQuestions)
                : View(GenderQuestionsViewPath, new EqualityQuestionsGenderViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("gender", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender)]
        public IActionResult Gender([FromRoute] Guid applicationId, EqualityQuestionsGenderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(GenderQuestionsViewPath, viewModel);
            }

            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            cacheStorageService.Set(cacheKey, (EqualityQuestionsModel)viewModel);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup, new { applicationId });
        }

        [HttpGet]
        [Route("ethnic-group", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup)]
        public IActionResult EthnicGroup([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicGroupQuestionsViewPath, (EqualityQuestionsEthnicGroupViewModel)equalityQuestions)
                : View(EthnicGroupQuestionsViewPath, new EqualityQuestionsEthnicGroupViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup)]
        public IActionResult EthnicGroup([FromRoute] Guid applicationId, EqualityQuestionsEthnicGroupViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(EthnicGroupQuestionsViewPath, viewModel);
            }

            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            var equalityQuestions = cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

            if (equalityQuestions is not null)
            {
                equalityQuestions.EthnicGroup = (EthnicGroup)Enum.Parse(typeof(EthnicGroup), viewModel.EthnicGroup!, true);

                cacheStorageService.Set(cacheKey, equalityQuestions);

                return RedirectToRoute(RouteNamesHelperService.GetEqualityFlowEthnicSubGroupRoute(equalityQuestions.EthnicGroup), new { applicationId });
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender, new { applicationId });
        }

        [HttpGet]
        [Route("ethnic-group/white", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public IActionResult EthnicGroupWhite([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupWhiteQuestionsViewPath, (EqualityQuestionsEthnicSubGroupWhiteViewModel)equalityQuestions)
                : View(EthnicSubGroupWhiteQuestionsViewPath, new EqualityQuestionsEthnicSubGroupWhiteViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/white", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public IActionResult EthnicGroupWhite([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupWhiteViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupWhiteQuestionsViewPath, viewModel)
                : UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/mixed", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
        public IActionResult EthnicGroupMixed([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupMixedQuestionsViewPath, (EqualityQuestionsEthnicSubGroupMixedViewModel)equalityQuestions)
                : View(EthnicSubGroupMixedQuestionsViewPath, new EqualityQuestionsEthnicSubGroupMixedViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/mixed", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
        public IActionResult EthnicGroupMixed([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupMixedViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupMixedQuestionsViewPath, viewModel)
                : UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/asian", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
        public IActionResult EthnicGroupAsian([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupAsianQuestionsViewPath, (EqualityQuestionsEthnicSubGroupAsianViewModel)equalityQuestions)
                : View(EthnicSubGroupAsianQuestionsViewPath, new EqualityQuestionsEthnicSubGroupAsianViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/asian", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
        public IActionResult EthnicGroupAsian([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupAsianViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupAsianQuestionsViewPath, viewModel)
                : UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/black", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
        public IActionResult EthnicGroupBlack([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupBlackQuestionsViewPath, (EqualityQuestionsEthnicSubGroupBlackViewModel)equalityQuestions)
                : View(EthnicSubGroupBlackQuestionsViewPath, new EqualityQuestionsEthnicSubGroupBlackViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/black", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
        public IActionResult EthnicGroupBlack([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupBlackViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupBlackQuestionsViewPath, viewModel)
                : UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("ethnic-group/other", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
        public IActionResult EthnicGroupOther([FromRoute] Guid applicationId)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();
            return equalityQuestions is not null
                ? View(EthnicSubGroupOtherQuestionsViewPath, (EqualityQuestionsEthnicSubGroupOtherViewModel)equalityQuestions)
                : View(EthnicSubGroupOtherQuestionsViewPath, new EqualityQuestionsEthnicSubGroupOtherViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/other", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
        public IActionResult EthnicGroupOther([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupOtherViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupOtherQuestionsViewPath, viewModel)
                : UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        [HttpGet]
        [Route("summary", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary)]
        public IActionResult Summary([FromRoute] Guid applicationId)
        {
            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            var equalityQuestions = cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

            if (equalityQuestions is null)
                return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender,
                    new { applicationId });

            return View(SummaryViewPath, (EqualityQuestionsSummaryViewModel)equalityQuestions);
        }

        private EqualityQuestionsModel? GetEqualityQuestionsFromCacheMemory()
        {
            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            var equalityQuestions = cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);
            return equalityQuestions ?? null;
        }

        private RedirectToRouteResult UpdateEqualityQuestionModel(Guid applicationId, string? subGroup, string? subGroupAnswer)
        {
            var equalityQuestions = GetEqualityQuestionsFromCacheMemory();

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

            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            cacheStorageService.Set(cacheKey, equalityQuestions);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary, new { applicationId });
        }
    }
}