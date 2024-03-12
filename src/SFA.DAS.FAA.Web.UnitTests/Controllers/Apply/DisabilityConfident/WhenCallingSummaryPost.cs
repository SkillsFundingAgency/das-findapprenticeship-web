using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.DisabilityConfident;


[TestFixture]
public class WhenCallingSummaryPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        bool isApplyUnderDisabilityConfidentSchemeRequired,
        UpdateDisabilityConfidenceApplicationCommandResult updateDisabilityConfidenceApplicationCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] DisabilityConfidentController controller)
    {
        var request = new DisabilityConfidentSummaryViewModel
        {
            ApplicationId = Guid.NewGuid(),
            IsApplyUnderDisabilityConfidentSchemeRequired = isApplyUnderDisabilityConfidentSchemeRequired,
            IsSectionCompleted = true
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<UpdateDisabilityConfidenceApplicationCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId) &&
                c.CandidateId.Equals(candidateId) &&
                c.DisabilityConfidenceSectionStatus.Equals(SectionStatus.Completed)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateDisabilityConfidenceApplicationCommandResult);

        var actual = await controller.PostSummary(request.ApplicationId, request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().BeEquivalentTo(RouteNames.Apply);
        }
    }
}