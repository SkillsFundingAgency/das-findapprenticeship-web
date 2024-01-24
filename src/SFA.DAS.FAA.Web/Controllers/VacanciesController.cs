using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Apply;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers;

public class VacanciesController(IMediator mediator, IDateTimeService dateTimeService, IValidator<GetVacancyDetailsRequest> validator) : Controller
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

    [Route("vacancies/{vacancyReference}", Name = RouteNames.Vacancies)]
    [HttpPost]
    public async Task<IActionResult> Apply([FromRoute] PostApplyRequest request)
    {
        var result = await mediator.Send(new ApplyCommand
        {
            VacancyReference = request.VacancyReference,
            CandidateId =  Guid.Parse("1DD26689-2997-4AEC-8FAF-62D4CE9F2155") //to be sourced from claims or similar following auth.
        });

        return RedirectToAction("Index", "Apply", new { result.ApplicationId });
    }
}