using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.DeleteQualifications;
using SFA.DAS.FAA.Application.Commands.UpdateQualifications;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Application.Commands.UpsertQualification;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications;
using SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;
using System.Reflection;
using FluentValidation;

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
                CandidateId = (Guid)User.Claims.CandidateId()!
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
                    CandidateId = (Guid)User.Claims.CandidateId()!
                });

                var viewModel = QualificationsViewModel.MapFromQueryResult(model.ApplicationId, result);
                return View(ViewName, viewModel);
            }

            if (model.ShowQualifications)
            {
                var completeSectionCommand = new UpdateQualificationsCommand
                {
                    CandidateId = (Guid)User.Claims.CandidateId()!,
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
                CandidateId = (Guid)User.Claims.CandidateId()!,
                IsComplete = model.DoYouWantToAddAnyQualifications == false
            });

            return RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
        }
        
        [HttpGet]
        [Route("apply/{applicationId}/qualifications/add/select-type", Name = RouteNames.ApplyApprenticeship.AddQualificationSelectType)]
        public async Task<IActionResult> AddQualificationSelectType(Guid applicationId)
        {
            var qualificationTypes = await mediator.Send(new GetQualificationTypesQuery{ ApplicationId = applicationId, CandidateId = (Guid)User.Claims.CandidateId()!});
            
            var viewModel = new AddQualificationSelectTypeViewModel
            {
                ApplicationId = applicationId,
                Qualifications = qualificationTypes.QualificationTypes.Select(c=>new AddQualificationSelectTypeViewModel.QualificationType
                {
                    QualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel(c.Name, c.Id),
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
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
                var qualificationTypes = await mediator.Send(new GetQualificationTypesQuery{ ApplicationId = model.ApplicationId, CandidateId = (Guid)User.Claims.CandidateId()!});
            
                var viewModel = new AddQualificationSelectTypeViewModel
                {
                    ApplicationId = model.ApplicationId,
                    Qualifications = qualificationTypes.QualificationTypes.Select(c=>new AddQualificationSelectTypeViewModel.QualificationType
                    {
                        QualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel(c.Name, c.Id),
                        Id = c.Id,
                        Name = c.Name
                    }).ToList(),
                    HasAddedQualifications = qualificationTypes.HasAddedQualifications,
                };
                ModelState.AddModelError(nameof(model.QualificationReferenceId), "Select your most recent qualification");

                return View(AddQualificationSelectTypeViewName,viewModel);
            }

            return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualification, new {ApplicationId = model.ApplicationId, QualificationReferenceId = model.QualificationReferenceId});
        }

        [HttpGet]
        [Route("apply/{applicationId}/qualifications/{qualificationReferenceId}/modify", Name = RouteNames.ApplyApprenticeship.AddQualification)]
        public async Task<IActionResult> ModifyQualification([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId, [FromQuery]Guid? id = null)
        {
            var result = await mediator.Send(new GetModifyQualificationQuery
            {
                ApplicationId = applicationId,
                QualificationReferenceId = qualificationReferenceId,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                QualificationId = id
            });

            if (result.QualificationType == null)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType,
                    new { applicationId });
            }

            var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel(result.QualificationType.Name, result.QualificationType.Id);
            var model = new AddQualificationViewModel
            {
                ApplicationId = applicationId,
                QualificationReferenceId = qualificationReferenceId,
                QualificationDisplayTypeViewModel = qualificationDisplayTypeViewModel,
                Subjects = !qualificationDisplayTypeViewModel.AllowMultipleAdd && id == null ? [] : result.Qualifications!.Select(c=>(SubjectViewModel)c).ToList(),
                Courses = result.Courses.Select(c=>(CourseDataListItem)c).ToList(),
                SingleQualificationId = id,
                QualificationType = result.QualificationType.Name,
                BackLinkUrl = id.HasValue
                    ? Url.RouteUrl(RouteNames.ApplyApprenticeship.Qualifications, new { applicationId })
                    : Url.RouteUrl(RouteNames.ApplyApprenticeship.AddQualificationSelectType, new { applicationId })
            };
            return View(AddQualificationViewName, model);
        }
        
        [HttpPost]
        [Route("apply/{applicationId}/qualifications/{qualificationReferenceId}/modify", Name = RouteNames.ApplyApprenticeship.AddQualification)]
        public async Task<IActionResult> ModifyQualification(
            IValidator<AddQualificationViewModel> validator,
            AddQualificationViewModel model)
        {
            await validator.ValidateAndUpdateModelStateAsync(model, ModelState);
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetModifyQualificationQuery
                {
                    ApplicationId = model.ApplicationId,
                    QualificationReferenceId = model.QualificationReferenceId,
                    CandidateId = (Guid)User.Claims.CandidateId()!,
                    QualificationId = model.SingleQualificationId,
                });
                var qualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel(result.QualificationType.Name, result.QualificationType.Id);
                
                model.QualificationDisplayTypeViewModel = qualificationDisplayTypeViewModel;
                model.Courses = result.Courses.Select(c => (CourseDataListItem)c).ToList();
                return View(AddQualificationViewName, model);
            }

            await mediator.Send(new UpsertQualificationCommand
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                ApplicationId = model.ApplicationId,
                QualificationReferenceId = model.QualificationReferenceId,
                Subjects = model.Subjects
                    .Where(c => !string.IsNullOrEmpty(c.Name) || (model.IsApprenticeship && !string.IsNullOrEmpty(c.Name)) || c is { IsDeleted: true, Id: not null })
                    .Select((subjectViewModel, index) => new PostUpsertQualificationsApiRequest.Subject
                    {
                        Grade = subjectViewModel.Grade,
                        Name = subjectViewModel.Name,
                        Id = subjectViewModel.Id ?? Guid.NewGuid(),
                        AdditionalInformation = subjectViewModel.Level ?? subjectViewModel.AdditionalInformation,
                        IsPredicted = subjectViewModel.IsPredicted.GetValueOrDefault(),
                        QualificationOrder = (short)(index + 1),
                        IsDeleted = subjectViewModel.IsDeleted
                    })
                    .ToList()
            });
            
            return RedirectToRoute(RouteNames.ApplyApprenticeship.Qualifications,
                new { model.ApplicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/qualifications/delete/{qualificationReferenceId}", Name = RouteNames.ApplyApprenticeship.DeleteQualifications)]
        public async Task<IActionResult> DeleteQualifications([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId, [FromQuery] Guid? id = null)
        {
            var result = await mediator.Send(new GetDeleteQualificationsQuery
            {
                ApplicationId = applicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                QualificationType = qualificationReferenceId,
                Id = id
            });

            if(result is null) return RedirectToRoute(RouteNames.Apply, new { applicationId });

            var viewModel = DeleteQualificationsViewModel.MapFromQueryResult(applicationId, qualificationReferenceId, id, result);

            return View(DeleteQualificationsViewName, viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/qualifications/delete/{qualificationReferenceId}", Name = RouteNames.ApplyApprenticeship.DeleteQualifications)]
        public async Task<IActionResult> DeleteQualifications(DeleteQualificationsViewModel viewModel)
        {
            await mediator.Send(new DeleteQualificationsCommand
            {
                ApplicationId = viewModel.ApplicationId,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                QualificationReferenceId = viewModel.QualificationReferenceId,
                Id = viewModel.Id
            });

            return RedirectToRoute(RouteNames.ApplyApprenticeship.Qualifications, new { viewModel.ApplicationId });
        }
    }
}
