using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.DisabilityConfident;

[TestFixture]
public class WhenPostingRequest
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
        Guid candidateId,
        Guid applicationId,
        DisabilityConfidentViewModel request,
        Mock<IValidator<DisabilityConfidentViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] DisabilityConfidentController controller)
    {
        // arrange
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
            
        validator
            .Setup(x => x.ValidateAsync(It.Is<DisabilityConfidentViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, applicationId, request) as RedirectToRouteResult;

        // assert
        using var scope = new AssertionScope();
        actual.Should().NotBeNull();
        actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.DisabilityConfidentConfirmation);
        actual?.RouteValues.Should().NotBeEmpty();
    }
}