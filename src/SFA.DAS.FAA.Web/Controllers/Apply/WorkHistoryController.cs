using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;
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
                foreach (var validationFailure in validation.Errors.Where(err => err.PropertyName != nameof(request.AddJob)))
                {
                    request.ErrorDictionary.Add(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }
            }

            request.BackLinkUrl = Url.RouteUrl(RouteNames.Apply,
                new GetIndexRequest { VacancyReference = request.VacancyReference });
            return View(ViewPath, request);
        }

        [HttpPost]
        [Route("vacancies/{vacancyReference}/apply/workhistory/{applicationId}", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public async Task<IActionResult> Post(AddWorkHistoryRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                foreach (var validationFailure in validation.Errors)
                {
                    request.ErrorDictionary.Add(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }                
                return View(ViewPath, request);
            }

            var command = new UpdateApplicationCommand
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