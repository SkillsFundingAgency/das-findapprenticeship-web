using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.WhatInterestsYou;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Reflection;
using System;
using SFA.DAS.FAA.Application.Commands.Qualifications;

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
                return View(ViewName, model);
            }

            if (model.DoYouWantToAddAnyQualifications is true)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.AddQualificationSelectType, new { model.ApplicationId });
            }

            await mediator.Send(new UpdateQualificationsCommand
            {
                ApplicationId = model.ApplicationId,
                CandidateId = User.Claims.CandidateId(),
                IsComplete = model.IsSectionCompleted ?? false
            });

            return RedirectToRoute(RouteNames.Apply, new { model.ApplicationId });
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
