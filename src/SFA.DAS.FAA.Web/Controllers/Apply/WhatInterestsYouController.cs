using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class WhatInterestsYouController : Controller
    {
        private const string _viewName = "~/Views/apply/WhatInterestsYou/Index.cshtml";

        [Route("apply/{applicationId}/what-interests-you", Name = RouteNames.ApplyApprenticeship.WhatInterestsYou)]
        public IActionResult Index([FromRoute] Guid applicationId)
        {
            var viewModel = new WhatInterestsYouViewModel
            {
                ApplicationId = applicationId,
                StandardName = "business administration", //todo: source these
                EmployerName = "King Recruitment"
            };

            return View(_viewName, viewModel);
        }

        [Route("apply/{applicationId}/what-interests-you", Name = RouteNames.ApplyApprenticeship.WhatInterestsYou)]
        [HttpPost]
        public IActionResult Index([FromRoute] Guid applicationId, WhatInterestsYouViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new WhatInterestsYouViewModel
                {
                    ApplicationId = applicationId,
                    StandardName = "business administration", //todo: source these
                    EmployerName = "King Recruitment"
                };

                return View(_viewName, viewModel);
            }


            var viewModel2 = new WhatInterestsYouViewModel
            {
                ApplicationId = applicationId,
                StandardName = "business administration", //todo: source these
                EmployerName = "King Recruitment"
            };

            return View(_viewName, viewModel2);
        }

    }
}
