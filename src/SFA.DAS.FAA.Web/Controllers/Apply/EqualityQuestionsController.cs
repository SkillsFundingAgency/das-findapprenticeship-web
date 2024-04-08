using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class EqualityQuestionsController(
        IDataProtectorService dataProtectorService,
        ICacheStorageService cacheStorageService) : Controller
    {
        private const string GenderQuestionsViewPath = "~/Views/apply/EqualityQuestions/Gender.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/EqualityQuestions/gender", Name = RouteNames.ApplyApprenticeship.EqualityFlowGender)]
        public IActionResult Gender([FromRoute] Guid applicationId)
        {
            return View(GenderQuestionsViewPath, new EqualityGenderViewModel { ApplicationId = applicationId });
        }

        [HttpPost]
        [Route("apply/{applicationId}/EqualityQuestions/gender", Name = RouteNames.ApplyApprenticeship.EqualityFlowGender)]
        public IActionResult Gender([FromRoute] Guid applicationId, EqualityGenderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ApplicationId = applicationId;
                return View(GenderQuestionsViewPath, viewModel);
            }

            var dataProtectorKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}",
                User.Claims.GovIdentifier());

            cacheStorageService.Set(
                $"{dataProtectorKey}-{CacheKeys.EqualityQuestionsGender}",
                JsonConvert.SerializeObject(viewModel),
                null,
                null);

            var dataProtectorEncodedKey = dataProtectorService.EncodedData(dataProtectorKey);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.EqualityFlowEthnicGroup, new { applicationId, key = dataProtectorEncodedKey });
        }
    }
}
