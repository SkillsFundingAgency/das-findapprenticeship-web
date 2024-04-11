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
    public class EqualityQuestionsController(ICacheStorageService cacheStorageService) : Controller
    {
        private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";
        private const string GenderQuestionsViewPath = "~/Views/apply/EqualityQuestions/Gender.cshtml";
        private const string EthnicGroupQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicGroup.cshtml";
        private const string EthnicSubGroupWhiteQuestionsViewPath = "~/Views/apply/EqualityQuestions/EthnicSubGroupWhite.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/equality-questions/gender", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender)]
        public IActionResult Gender([FromRoute] Guid applicationId)
        {
            return View(GenderQuestionsViewPath, new EqualityQuestionsGenderViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("apply/{applicationId}/equality-questions/gender", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender)]
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
        [Route("apply/{applicationId}/equality-questions/ethnic-group", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup)]
        public IActionResult EthnicGroup([FromRoute] Guid applicationId)
        {
            return View(EthnicGroupQuestionsViewPath, new EqualityQuestionsEthnicGroupViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("apply/{applicationId}/equality-questions/ethnic-group", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup)]
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
        [Route("apply/{applicationId}/equality-questions/ethnic-group/white", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public IActionResult EthnicGroupWhite([FromRoute] Guid applicationId)
        {
            return View(EthnicSubGroupWhiteQuestionsViewPath, new EqualityQuestionsEthnicSubGroupViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("apply/{applicationId}/equality-questions/ethnic-group/white", Name = RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public IActionResult EthnicGroupWhite([FromRoute] Guid applicationId, EqualityQuestionsEthnicSubGroupViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(EthnicSubGroupWhiteQuestionsViewPath, viewModel);
            }

            var cacheKey = string.Format($"{Key}", User.Claims.GovIdentifier());
            var equalityQuestions = cacheStorageService.Get<EqualityQuestionsModel>(cacheKey);

            if (equalityQuestions is not null)
            {
                equalityQuestions.EthnicSubGroup = (EthnicSubGroup)Enum.Parse(typeof(EthnicSubGroup), viewModel.EthnicSubGroup!, true);
                equalityQuestions.OtherEthnicSubGroupAnswer = viewModel.OtherEthnicSubGroupAnswer;

                cacheStorageService.Set(cacheKey, equalityQuestions);

                return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary, new { applicationId });
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender, new { applicationId });
        }
    }
}
