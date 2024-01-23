using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpsertApplication;
using SFA.DAS.FAA.Domain.Apply.UpsertApplication.Enums;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    public class WorkHistoryController(IMediator mediator, IValidator<AddWorkHistoryRequest> validator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";

        [HttpGet]
        [Route("vacancies/{vacancyReference}/apply/workhistory/{applicationId}", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public async Task<IActionResult> Get(AddWorkHistoryRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                //upcoming stories will cover this section. 404 not found.
            }

            return View(ViewPath, new AddWorkHistoryRequest
            {
                VacancyReference = request.VacancyReference,
                ApplicationId = request.ApplicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new GetIndexRequest { VacancyReference = request.VacancyReference })
            });
        }

        [HttpPost]
        [Route("vacancies/{vacancyReference}/apply/workhistory/{applicationId}", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public async Task<IActionResult> Post(AddWorkHistoryRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                if (string.IsNullOrEmpty(request.AddJob))
                {
                    request.ErrorDictionary.Add(nameof(request.AddJob), "Select if you want to add any jobs");
                    return View(ViewPath, request);
                }
            }

            var command = new UpsertApplicationCommand
            {
                VacancyReference = request.VacancyReference,
                CandidateId = Guid.NewGuid(), // TODO: candidateId comes from Gov.One SignIn Integration.
                ApplicationId = request.ApplicationId,
                WorkHistorySectionStatus = request.AddJob is "Yes" ? SectionStatus.InProgress : SectionStatus.Completed
            };

            await mediator.Send(command);

            return request.AddJob.Equals("Yes") 
                ? RedirectToRoute("/") //TODO: Redirect the user to Add Job Page.
                : RedirectToRoute(RouteNames.Apply, new GetIndexRequest { VacancyReference = request.VacancyReference });
        }
    }
}