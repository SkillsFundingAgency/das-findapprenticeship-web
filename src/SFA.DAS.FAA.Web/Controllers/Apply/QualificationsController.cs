using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Application.Commands.Qualifications;
using SFA.DAS.FAA.Application.Commands.UpsertQualification;
using SFA.DAS.FAA.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class QualificationsController(IMediator mediator) : Controller
    {
        private const string ViewName = "~/Views/apply/Qualifications/Index.cshtml";
        private const string AddQualificationSelectTypeViewName = "~/Views/apply/Qualifications/AddQualificationSelectType.cshtml";
        private const string AddQualificationViewName = "~/Views/apply/Qualifications/AddQualification.cshtml";
        private const string DeleteQualificationsViewName = "~/Views/apply/Qualifications/DeleteQualifications.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/qualifications", Name = RouteNames.ApplyApprenticeship.Qualifications)]
        public async Task<IActionResult> Get(Guid applicationId)
        {
            var result = await mediator.Send(new GetQualificationsQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = QualificationsViewModel.MapFromQueryResult(applicationId, result);

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

                var viewModel = QualificationsViewModel.MapFromQueryResult(model.ApplicationId, result);
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
            var qualificationTypes = await mediator.Send(new GetQualificationTypesQuery{ ApplicationId = applicationId});
            
            var viewModel = new AddQualificationSelectTypeViewModel
            {
                ApplicationId = applicationId,
                Qualifications = qualificationTypes.QualificationTypes,
                HasAddedQualifications = qualificationTypes.HasAddedQualifications,
            };

            return View(AddQualificationSelectTypeViewName, viewModel);
        }
        [HttpPost]
        [Route("apply/{applicationId}/qualifications/add/select-type", Name = RouteNames.ApplyApprenticeship.AddQualificationSelectType)]
        public async Task<IActionResult> AddQualificationSelectTypePost(AddQualificationSelectTypeViewModel model)
        {
            if (model.QualificationReferenceId == Guid.Empty)
            {
                var qualificationTypes = await mediator.Send(new GetQualificationTypesQuery{ ApplicationId = model.ApplicationId});
            
                var viewModel = new AddQualificationSelectTypeViewModel
                {
                    ApplicationId = model.ApplicationId,
                    Qualifications = qualificationTypes.QualificationTypes,
                    HasAddedQualifications = qualificationTypes.HasAddedQualifications,
                };
                ModelState.AddModelError(nameof(model.QualificationReferenceId), "Select your most recent qualification");

                return View(AddQualificationSelectTypeViewName,viewModel);
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualification, new {ApplicationId = model.ApplicationId, QualificationReferenceId = model.QualificationReferenceId});
        }

        [HttpGet]
        [Route("apply/{applicationId}/qualifications/{qualificationReferenceId}/modify", Name = RouteNames.ApplyApprenticeship.AddQualification)]
        public async Task<IActionResult> ModifyQualification([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId)
        {
            var result = await mediator.Send(new GetModifyQualificationQuery
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
        [Route("apply/{applicationId}/qualifications/{qualificationReferenceId}/modify", Name = RouteNames.ApplyApprenticeship.AddQualification)]
        public async Task<IActionResult> ModifyQualification(AddQualificationViewModel model)
        {
            //TODO - Add validation
            await mediator.Send(new UpsertQualificationCommand
            {
                CandidateId = User.Claims.CandidateId(),
                ApplicationId = model.ApplicationId,
                QualificationReferenceId = model.QualificationReferenceId,
                Subjects = model.Subjects.Select(c => new PostUpsertQualificationsApiRequest.Subject
                {
                    Grade = c.Grade,
                    Name = c.Name,
                    Id = c.Id == Guid.Empty ? Guid.NewGuid() : c.Id,
                    AdditionalInformation = c.Level,
                    IsPredicted = c.IsPredicted
                }).ToList()
            });
            
            return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType,
                new { model.ApplicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/qualifications/delete/{qualificationReferenceId}", Name = RouteNames.ApplyApprenticeship.DeleteQualifications)]
        public async Task<IActionResult> DeleteQualifications([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId)
        {
            return View(DeleteQualificationsViewName);
        }
    }
}
