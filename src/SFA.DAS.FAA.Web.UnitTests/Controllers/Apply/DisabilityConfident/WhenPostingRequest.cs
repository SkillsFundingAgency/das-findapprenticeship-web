using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.DisabilityConfident
{
    [TestFixture]
    public class WhenPostingRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
            Guid candidateId,
            Guid applicationId,
            DisabilityConfidentViewModel request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DisabilityConfidentController controller)
        {
            //arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator.Setup(x => x.Send(It.Is<UpdateDisabilityConfidentCommand>(c =>
                        c.ApplicationId == applicationId &&
                        c.CandidateId == candidateId &&
                        c.ApplyUnderDisabilityConfidentScheme == request.ApplyUnderDisabilityConfidentScheme),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            var actual = await controller.Post(applicationId, request) as RedirectToRouteResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation);
            actual?.RouteValues.Should().NotBeEmpty();
        }
    }
}
