using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.InterviewAdjustments;

public class WhenCallingPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_Summary(
        Guid candidateId,
        string adjustmentsInput,
        UpdateInterviewAdjustmentsCommandResult createInterviewAdjustmentsQueryResult,
        Mock<IValidator<InterviewAdjustmentsViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] InterviewAdjustmentsController controller)
    {
        // arrange
        var request = new InterviewAdjustmentsViewModel
        {
            ApplicationId = Guid.NewGuid(),
            InterviewAdjustmentsDescription = adjustmentsInput,
            DoYouWantInterviewAdjustments = true
        };
        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<UpdateInterviewAdjustmentsCommand>(c =>
        c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createInterviewAdjustmentsQueryResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<InterviewAdjustmentsViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, request.ApplicationId, request) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary);
        }
    }
}
