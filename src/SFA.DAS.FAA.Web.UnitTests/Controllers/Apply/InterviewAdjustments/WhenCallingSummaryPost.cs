using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.InterviewAdjustments;

[TestFixture]
public class WhenCallingSummaryPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        string adjustmentsInput,
        UpdateInterviewAdjustmentsCommandResult updateInterviewAdjustmentsCommandResult,
        Mock<IValidator<InterviewAdjustmentSummaryViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] InterviewAdjustmentsController controller)
    {
        // arrange
        var request = new InterviewAdjustmentSummaryViewModel
        {
            ApplicationId = Guid.NewGuid(),
            SupportRequestAnswer = adjustmentsInput,
            IsSupportRequestRequired = true,
            IsSectionCompleted = true
        };

        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<UpdateInterviewAdjustmentsCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateInterviewAdjustmentsCommandResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<InterviewAdjustmentSummaryViewModel>(m => m == request), CancellationToken.None))
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