using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Apply;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers;

public class VacanciesController(
    IMediator mediator,
    IDateTimeService dateTimeService,
    IValidator<GetVacancyDetailsRequest> validator) : Controller
{
    [Route("vacancies/{vacancyReference}", Name = RouteNames.Vacancies)]
    public async Task<IActionResult> Vacancy([FromRoute] GetVacancyDetailsRequest request)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            //upcoming stories will be cover this section. 404 not found.
        }

        var result = await mediator.Send(new GetApprenticeshipVacancyQuery
        {
            VacancyReference = request.VacancyReference
        });

        var viewModel = new VacancyDetailsViewModel().MapToViewModel(dateTimeService, result);
        return View(viewModel);
    }

    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("vacancies/{vacancyReference}", Name = RouteNames.Vacancies)]
    [HttpPost]
    public async Task<IActionResult> Apply([FromRoute] PostApplyRequest request)
    {
        var result = await mediator.Send(new ApplyCommand
        {
            VacancyReference = request.VacancyReference,
            CandidateId = User.Claims.CandidateId() 
        });

        return RedirectToAction("Index", "Apply", new { result.ApplicationId });
    }
}