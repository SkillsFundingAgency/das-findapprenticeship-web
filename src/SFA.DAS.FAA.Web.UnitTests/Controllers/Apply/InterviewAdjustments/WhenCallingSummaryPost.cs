using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.InterviewAdjustments;

[TestFixture]
public class WhenCallingSummaryPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        string adjustmentsInput,
        UpdateInterviewAdjustmentsCommandResult updateInterviewAdjustmentsCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] InterviewAdjustmentsController controller)
    {
        var request = new InterviewAdjustmentSummaryViewModel
        {
            ApplicationId = Guid.NewGuid(),
            SupportRequestAnswer = adjustmentsInput,
            IsSupportRequestRequired = true,
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

        mediator.Setup(x => x.Send(It.Is<UpdateInterviewAdjustmentsCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateInterviewAdjustmentsCommandResult);

        var actual = await controller.PostSummary(request.ApplicationId, request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().BeEquivalentTo(RouteNames.Apply);
        }
    }
}