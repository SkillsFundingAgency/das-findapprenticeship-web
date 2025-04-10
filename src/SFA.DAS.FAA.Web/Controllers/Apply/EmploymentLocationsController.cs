using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocation;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class EmploymentLocationsController(IMediator mediator) : Controller
{
    private const string AddViewPath = "~/Views/apply/EmploymentLocations/AddEmploymentLocations.cshtml";

    [HttpGet]
    [Route("apply/{applicationId:guid}/employmentLocation", Name = RouteNames.ApplyApprenticeship.AddEmploymentLocations)]
    public async Task<IActionResult> AddEmploymentLocations([FromRoute] Guid applicationId)
    {
        var result = await mediator.Send(new GetEmploymentLocationQuery(applicationId, (Guid)User.Claims.CandidateId()!));
        return View(AddViewPath, new AddEmploymentLocationsViewModel
        {
            Addresses = result.EmploymentLocation.Addresses,
            ApplicationId = applicationId,
        });
    }
}