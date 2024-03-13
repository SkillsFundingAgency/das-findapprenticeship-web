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
public class WhenCallingPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_Summary(
       Guid candidateId,
       string adjustmentsInput,
       UpdateInterviewAdjustmentsCommandResult createInterviewAdjustmentsQueryResult,
       [Frozen] Mock<IMediator> mediator,
       [Greedy] InterviewAdjustmentsController controller)
    {
        var request = new InterviewAdjustmentsViewModel
        {
            ApplicationId = Guid.NewGuid(),
            InterviewAdjustmentsDescription = adjustmentsInput,
            DoYouWantInterviewAdjustments = true
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
            .ReturnsAsync(createInterviewAdjustmentsQueryResult);

        var actual = await controller.Post(request.ApplicationId, request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary);
        }
    }
}
