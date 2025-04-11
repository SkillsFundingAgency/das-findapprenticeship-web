using MediatR;
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
            [FromQuery] string? vacancyId = null,
            SortOrder sortOrder = SortOrder.RecentlySaved)
        {
            var result = await mediator.Send(new GetSavedVacanciesQuery
            {
                CandidateId = (Guid)User.Claims.CandidateId()!,
                DeleteVacancyId = vacancyId
            });

            var viewModel = IndexViewModel.Map(result, dateTimeService, sortOrder);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{vacancyId}/delete", Name = RouteNames.DeleteSavedVacancy)]
        public async Task<IActionResult> DeleteSavedVacancy([FromRoute] string vacancyId)
        {
            await mediator.Send(new DeleteSavedVacancyCommand
            {
                VacancyId = vacancyId,
                CandidateId = (Guid)User.Claims.CandidateId()!
            });

            return RedirectToRoute(RouteNames.SavedVacancies, new
            {
                VacancyId = vacancyId
            });
        }
    }
}
