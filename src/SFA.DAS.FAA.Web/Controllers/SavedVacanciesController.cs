﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy;
using SFA.DAS.FAA.Application.Queries.GetSavedVacancies;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedVacancies;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("saved-vacancies")]
    public class SavedVacanciesController(IMediator mediator, IDateTimeService dateTimeService) : Controller
    {
        [Route("", Name = RouteNames.SavedVacancies)]
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] string? vacancyReference = null,
            SortOrder sortOrder = SortOrder.DateSaved)
        {
            var result = await mediator.Send(new GetSavedVacanciesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                DeleteVacancyReference = vacancyReference
            });

            var viewModel = IndexViewModel.Map(result, dateTimeService, sortOrder);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{vacancyReference}/delete", Name = RouteNames.DeleteSavedVacancy)]
        public async Task<IActionResult> DeleteSavedVacancy([FromRoute] string vacancyReference)
        {
            await mediator.Send(new DeleteSavedVacancyCommand
            {
                VacancyId = vacancyReference,
                CandidateId = (Guid)User.Claims.CandidateId()!,
                DeleteAllByVacancyReference = true
            });

            return RedirectToRoute(RouteNames.SavedVacancies, new
            {
                VacancyReference = vacancyReference
            });
        }
    }
}
