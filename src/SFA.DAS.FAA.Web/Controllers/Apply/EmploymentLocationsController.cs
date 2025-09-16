using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.EmploymentLocations.Update;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class EmploymentLocationsController(IMediator mediator) : Controller
{
    private const string ListViewPath = "~/Views/apply/EmploymentLocations/List.cshtml";
    private const string SummaryViewPath = "~/Views/apply/EmploymentLocations/Summary.cshtml";

    [HttpGet]
    [Route("apply/{applicationId:guid}/employmentLocations", Name = RouteNames.ApplyApprenticeship.AddEmploymentLocations)]
    public async Task<IActionResult> AddEmploymentLocations([FromRoute] Guid applicationId, [FromQuery] bool isEdit = false)
    {
        var result = await mediator.Send(new GetEmploymentLocationsQuery(applicationId, (Guid)User.Claims.CandidateId()!));

        if (!isEdit && result.EmploymentLocation.Addresses.Exists(add => add.IsSelected))
        {
            return RedirectToRoute(RouteNames.ApplyApprenticeship.EmploymentLocationsSummary,
                new { ApplicationId = applicationId, isEdit });
        }

        var viewModel = (AddEmploymentLocationsViewModel)result;
        viewModel.ApplicationId = applicationId;
        return View(ListViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId:guid}/employmentLocations", Name = RouteNames.ApplyApprenticeship.AddEmploymentLocations)]
    public async Task<IActionResult> AddEmploymentLocations(
        [FromServices] IValidator<AddEmploymentLocationsViewModel> validator,
        [FromRoute] Guid applicationId,
        AddEmploymentLocationsViewModel viewModel,
        [FromQuery] bool isEdit = false)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        
        if (!ModelState.IsValid)
        {
            var result = await mediator.Send(new GetEmploymentLocationsQuery(applicationId, (Guid)User.Claims.CandidateId()!));
            // Reset the selection for all addresses in one step
            result.EmploymentLocation.Addresses.ForEach(address => address.IsSelected = false);

            viewModel = result;
            viewModel.ApplicationId = applicationId;
            return View(ListViewPath, viewModel);
        }
        await mediator.Send(new UpdateEmploymentLocationsCommand
        {
            CandidateId = (Guid)User.Claims.CandidateId()!,
            ApplicationId = applicationId,
            SelectedAddressIds = viewModel.SelectedAddressIds!,
            EmploymentLocationSectionStatus = SectionStatus.InProgress
        });

        return RedirectToRoute(RouteNames.ApplyApprenticeship.EmploymentLocationsSummary, new { applicationId, isEdit });
    }

    [HttpGet]
    [Route("apply/{applicationId:guid}/employmentLocations/summary", Name = RouteNames.ApplyApprenticeship.EmploymentLocationsSummary)]
    public async Task<IActionResult> Summary([FromRoute] Guid applicationId, [FromQuery] bool isEdit = false)
    {
        var result = await mediator.Send(new GetEmploymentLocationsQuery(applicationId, (Guid)User.Claims.CandidateId()!));

        var viewModel = (EmploymentLocationsSummaryViewModel)result;
        viewModel.BackLinkUrl = isEdit
            ? Url.RouteUrl(RouteNames.ApplyApprenticeship.AddEmploymentLocations, new {applicationId, isEdit})
            : Url.RouteUrl(RouteNames.Apply, new {applicationId});
        viewModel.ApplicationId = applicationId;
        viewModel.IsSectionCompleted = result.IsSectionCompleted;

        return View(SummaryViewPath, viewModel);
    }

    [HttpPost]
    [Route("apply/{applicationId:guid}/employmentLocations/summary", Name = RouteNames.ApplyApprenticeship.EmploymentLocationsSummary)]
    public async Task<IActionResult> Summary(
        [FromServices] IValidator<EmploymentLocationsSummaryViewModel> validator,
        [FromRoute] Guid applicationId,
        EmploymentLocationsSummaryViewModel viewModel,
        [FromQuery] bool isEdit = false)
    {
        await validator.ValidateAndUpdateModelStateAsync(viewModel, ModelState);
        if (!ModelState.IsValid)
        {
            var result = await mediator.Send(new GetEmploymentLocationsQuery(applicationId, (Guid)User.Claims.CandidateId()!));

            viewModel = (EmploymentLocationsSummaryViewModel)result;
            viewModel.BackLinkUrl = isEdit
                ? Url.RouteUrl(RouteNames.ApplyApprenticeship.AddEmploymentLocations, new { applicationId, isEdit })
                : Url.RouteUrl(RouteNames.Apply, new { applicationId });
            viewModel.ApplicationId = applicationId;
            viewModel.IsSectionCompleted = result.IsSectionCompleted;

            return View(SummaryViewPath, viewModel);
        }

        await mediator.Send(new UpdateEmploymentLocationsCommand
        {
            CandidateId = (Guid)User.Claims.CandidateId()!,
            ApplicationId = applicationId,
            EmploymentLocationSectionStatus = viewModel.IsSectionCompleted!.Value ? SectionStatus.Completed : SectionStatus.Incomplete
        });

        return RedirectToRoute(RouteNames.Apply, new { applicationId });
    }
}