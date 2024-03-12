using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using System;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class QualificationsController(IMediator mediator) : Controller
    {
        private const string ViewName = "~/Views/apply/Qualifications/Index.cshtml";
        private const string AddQualificationSelectTypeViewName = "~/Views/apply/Qualifications/AddQualificationSelectType.cshtml";

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
            if (!ModelState.IsValid)
            {
                return View(ViewName, viewModel);
            }

            if (viewModel.DoYouWantToAddAnyQualifications is true)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType, new { viewModel.ApplicationId });
            }

            return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
        }
        
        [HttpGet]
        [Route("apply/{applicationId}/qualifications/add/select-type", Name = RouteNames.ApplyApprenticeship.AddQualificationSelectType)]
        public async Task<IActionResult> AddQualificationSelectType(Guid applicationId)
        {
            var viewModel = new AddQualificationSelectTypeViewModel
            {
                ApplicationId = applicationId
            };

            return View(AddQualificationSelectTypeViewName, viewModel);
        }
    }
}
