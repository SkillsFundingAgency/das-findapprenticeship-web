using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class QualificationsController(IMediator mediator) : Controller
    {
        private const string ViewName = "~/Views/apply/Qualifications/Index.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/qualifications", Name = RouteNames.ApplyApprenticeship.Qualifications)]
        public async Task<IActionResult> Get(Guid applicationId)
        {
            var viewModel = new QualificationsViewModel
            {
                ApplicationId = applicationId,
                DoYouWantToAddAnyQualifications = null
            };

            return View(ViewName, viewModel);
        }


        [HttpPost]
        [Route("apply/{applicationId}/qualifications", Name = RouteNames.ApplyApprenticeship.Qualifications)]
        public async Task<IActionResult> Post(QualificationsViewModel viewModel)
        {
            return View(ViewName, viewModel);
        }
    }
}
