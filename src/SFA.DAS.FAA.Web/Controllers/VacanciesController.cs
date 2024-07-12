using System.Net;
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
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Controllers;

public class VacanciesController(
    IMediator mediator,
    IDateTimeService dateTimeService,
    ICacheStorageService cacheStorageService,
    IValidator<GetVacancyDetailsRequest> validator) : Controller
{
    [Route("apprenticeship/{vacancyReference}", Name = RouteNames.Vacancies, Order = 1)]
    [Route("apprenticeship/reference/{vacancyReference}", Name = RouteNames.VacanciesReference, Order = 1)]
    public async Task<IActionResult> Vacancy([FromRoute] GetVacancyDetailsRequest request)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return NotFound();
        }

        if (!request.VacancyReference.StartsWith("VAC", StringComparison.CurrentCultureIgnoreCase))
        {
            request.VacancyReference = $"VAC{request.VacancyReference}";
        }

        var result = await mediator.Send(new GetApprenticeshipVacancyQuery
        {
            VacancyReference = request.VacancyReference,
            CandidateId = User.Claims.CandidateId().Equals(null) ? null
                : User.Claims.CandidateId()!.ToString()
        });

        var viewModel = new VacancyDetailsViewModel().MapToViewModel(dateTimeService, result);
        viewModel.ShowAccountCreatedBanner =
            await NotificationBannerService.ShowAccountCreatedBanner(cacheStorageService,
                $"{User.Claims.GovIdentifier()}-{CacheKeys.AccountCreated}");

        return View(viewModel);
    }

    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("apprenticeship/{vacancyReference}", Name = RouteNames.Vacancies)]
    [Route("apprenticeship/reference/{vacancyReference}", Name = RouteNames.VacanciesReference, Order = 1)]
    [HttpPost]
    public async Task<IActionResult> Apply([FromRoute] PostApplyRequest request)
    {
        if (!request.VacancyReference.StartsWith("VAC", StringComparison.CurrentCultureIgnoreCase))
        {
            request.VacancyReference = $"VAC{request.VacancyReference}";
        }
        
        var result = await mediator.Send(new ApplyCommand
        {
            VacancyReference = request.VacancyReference,
            CandidateId = (Guid)User.Claims.CandidateId()! 
        });

        return RedirectToAction("Index", "Apply", new { result.ApplicationId });
    }
}