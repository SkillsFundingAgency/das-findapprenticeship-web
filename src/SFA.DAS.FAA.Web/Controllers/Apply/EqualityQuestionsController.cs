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

        [HttpGet]
        [Route("gender", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender)]
        public IActionResult Gender([FromRoute] Guid applicationId)
        {
            return View(GenderQuestionsViewPath, new EqualityQuestionsGenderViewModel { ApplicationId = applicationId });
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
            return View(EthnicGroupQuestionsViewPath, new EqualityQuestionsEthnicGroupViewModel { ApplicationId = applicationId });
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
            return View(EthnicSubGroupWhiteQuestionsViewPath, new EqualityQuestionsEthnicSubGroupWhiteViewModel { ApplicationId = applicationId });
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
            return View(EthnicSubGroupMixedQuestionsViewPath, new EqualityQuestionsEthnicSubGroupMixedViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("ethnic-group/mixed", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
        public IActionResult EthnicGroupMixed([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupMixedViewModel viewModel)
        {
            return !ModelState.IsValid
                ? View(EthnicSubGroupMixedQuestionsViewPath, viewModel)
                : UpdateEqualityQuestionModel(applicationId, viewModel.EthnicSubGroup, viewModel.OtherEthnicSubGroupAnswer);
        }

        private RedirectToRouteResult UpdateEqualityQuestionModel(Guid applicationId, string? subGroup, string? subGroupAnswer)
        {
            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            var equalityQuestions = cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

            if (equalityQuestions is null)
                return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender,
                    new {applicationId});

            equalityQuestions.EthnicSubGroup = (EthnicSubGroup)Enum.Parse(typeof(EthnicSubGroup), subGroup!, true);
            equalityQuestions.OtherEthnicSubGroupAnswer = subGroupAnswer;

            cacheStorageService.Set(cacheKey, equalityQuestions);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary, new { applicationId });
        }
    }
}