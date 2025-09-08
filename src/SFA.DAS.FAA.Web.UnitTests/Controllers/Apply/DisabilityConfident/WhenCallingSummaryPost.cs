using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.DisabilityConfident;

[TestFixture]
public class WhenCallingSummaryPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        bool isApplyUnderDisabilityConfidentSchemeRequired,
        UpdateDisabilityConfidenceApplicationCommandResult updateDisabilityConfidenceApplicationCommandResult,
        Mock<IValidator<DisabilityConfidentSummaryViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] DisabilityConfidentController controller)
    {
        // arrange
        var request = new DisabilityConfidentSummaryViewModel
        {
            ApplicationId = Guid.NewGuid(),
            IsApplyUnderDisabilityConfidentSchemeRequired = isApplyUnderDisabilityConfidentSchemeRequired,
            IsSectionCompleted = true
        };

        controller
            .AddControllerContext()
            .WithUser(candidateId);

        mediator.Setup(x => x.Send(It.Is<UpdateDisabilityConfidenceApplicationCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId) &&
                c.CandidateId.Equals(candidateId) &&
                c.IsSectionCompleted.Equals(SectionStatus.Completed)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateDisabilityConfidenceApplicationCommandResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<DisabilityConfidentSummaryViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.PostSummary(validator.Object, request.ApplicationId, request) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().BeEquivalentTo(RouteNames.Apply);
        }
    }
}