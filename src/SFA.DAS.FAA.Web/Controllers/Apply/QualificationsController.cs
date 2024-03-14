using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Application.Commands.Qualifications;
using SFA.DAS.FAA.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class QualificationsController(IMediator mediator) : Controller
    {
        private const string ViewName = "~/Views/apply/Qualifications/Index.cshtml";
        private const string AddQualificationSelectTypeViewName = "~/Views/apply/Qualifications/AddQualificationSelectType.cshtml";
        private const string AddQualificationViewName = "~/Views/apply/Qualifications/AddQualification.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/qualifications", Name = RouteNames.ApplyApprenticeship.Qualifications)]
        public async Task<IActionResult> Get(Guid applicationId)
        {
            var result = await mediator.Send(new GetQualificationsQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = new QualificationsViewModel
            {
                ApplicationId = applicationId,
                DoYouWantToAddAnyQualifications = result.Qualifications.Count == 0 && result.IsSectionCompleted is true ? false : null,
                IsSectionCompleted = result.IsSectionCompleted,
                Qualifications = result.Qualifications.Select(x => (QualificationsViewModel.Qualification)x).ToList(),
                ShowQualifications = result.Qualifications.Count != 0
            };

            return View(ViewName, viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/qualifications", Name = RouteNames.ApplyApprenticeship.Qualifications)]
        public async Task<IActionResult> Post(QualificationsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetQualificationsQuery
                {
                    ApplicationId = model.ApplicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                var viewModel = new QualificationsViewModel
                {
                    ApplicationId = model.ApplicationId,
                    DoYouWantToAddAnyQualifications = result.Qualifications.Count == 0 && result.IsSectionCompleted is true ? false : null,
                    IsSectionCompleted = result.IsSectionCompleted,
                    Qualifications = result.Qualifications.Select(x => (QualificationsViewModel.Qualification)x).ToList(),
                    ShowQualifications = result.Qualifications.Count != 0
                };
                return View(ViewName, viewModel);
            }

            if (model.ShowQualifications)
            {
                var completeSectionCommand = new UpdateQualificationsCommand
                {
                    CandidateId = User.Claims.CandidateId(),
                    ApplicationId = model.ApplicationId,
                    IsComplete = model.IsSectionCompleted ?? false
                };

                await mediator.Send(completeSectionCommand);

                return RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
            }

            if (model.DoYouWantToAddAnyQualifications is true)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType, new { model.ApplicationId });
            }

            await mediator.Send(new UpdateQualificationsCommand
            {
                ApplicationId = model.ApplicationId,
                CandidateId = User.Claims.CandidateId(),
                IsComplete = model.DoYouWantToAddAnyQualifications == false
            });

            return RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
        }
        
        [HttpGet]
        [Route("apply/{applicationId}/qualifications/add/select-type", Name = RouteNames.ApplyApprenticeship.AddQualificationSelectType)]
        public async Task<IActionResult> AddQualificationSelectType(Guid applicationId)
        {
            var qualificationTypes = await mediator.Send(new GetQualificationTypesQuery());
            
            var viewModel = new AddQualificationSelectTypeViewModel
            {
                ApplicationId = applicationId,
                Qualifications = qualificationTypes.QualificationTypes
            };

            return View(AddQualificationSelectTypeViewName, viewModel);
        }
        [HttpPost]
        [Route("apply/{applicationId}/qualifications/add/select-type", Name = RouteNames.ApplyApprenticeship.AddQualificationSelectType)]
        public async Task<IActionResult> AddQualificationSelectTypePost(AddQualificationSelectTypeViewModel model)
        {
            if (model.QualificationReferenceId == Guid.Empty)
            {
                var qualificationTypes = await mediator.Send(new GetQualificationTypesQuery());
            
                var viewModel = new AddQualificationSelectTypeViewModel
                {
                    ApplicationId = model.ApplicationId,
                    Qualifications = qualificationTypes.QualificationTypes
                };
                ModelState.AddModelError(nameof(model.QualificationReferenceId), "Select your most recent qualification");

                return View(AddQualificationSelectTypeViewName,viewModel);
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualification, new {ApplicationId = model.ApplicationId, QualificationReferenceId = model.QualificationReferenceId});
        }

        [HttpGet]
        [Route("apply/{applicationId}/qualifications/add/{qualificationReferenceId}", Name = RouteNames.ApplyApprenticeship.AddQualification)]
        public async Task<IActionResult> AddQualification([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId)
        {
            var result = await mediator.Send(new GetAddQualificationQuery
            {
                ApplicationId = applicationId,
                QualificationReferenceId = qualificationReferenceId
            });

            if (result.QualificationType == null)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType,
                    new { applicationId });
            }
            
            var model = new AddQualificationViewModel
            {
                ApplicationId = applicationId,
                QualificationReferenceId = qualificationReferenceId,
                QualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel(result.QualificationType.Name)
            };
            return View(AddQualificationViewName, model);
        }
        
        [HttpPost]
        [Route("apply/{applicationId}/qualifications/add/{qualificationReferenceId}", Name = RouteNames.ApplyApprenticeship.AddQualification)]
        public async Task<IActionResult> AddQualification(AddQualificationViewModel model)
        {
            
            return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType,
                new { model.ApplicationId });
        }
    }
}
