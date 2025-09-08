using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;

public class WhenPostingRequest
{
    [Test, MoqAutoData]
    public async Task And_User_Has_No_Training_Courses_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        UpdateTrainingCoursesApplicationCommandResult result,
        Mock<IValidator<TrainingCoursesViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        // arrange
        var request = new TrainingCoursesViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyTrainingCourses = false,
        };
        controller
            .AddControllerContext()
            .WithUser(Guid.NewGuid())
            .WithClaim(CustomClaims.CandidateId, candidateId.ToString());
        mediator.Setup(x => x.Send(It.IsAny<UpdateTrainingCoursesApplicationCommandResult>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        validator
            .Setup(x => x.ValidateAsync(It.Is<TrainingCoursesViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, applicationId, request) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.Apply);
            actual?.RouteValues.Should().NotBeEmpty();
        }
    }

    [Test, MoqAutoData]
    public async Task And_User_Has_Training_Courses_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        UpdateTrainingCoursesApplicationCommandResult result,
        Mock<IValidator<TrainingCoursesViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        // arrange
        var request = new TrainingCoursesViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyTrainingCourses = true,
        };
        controller
            .AddControllerContext()
            .WithUser(Guid.NewGuid())
            .WithClaim(CustomClaims.CandidateId, candidateId.ToString());
        mediator.Setup(x => x.Send(It.IsAny<UpdateTrainingCoursesApplicationCommandResult>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        validator
            .Setup(x => x.ValidateAsync(It.Is<TrainingCoursesViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, applicationId, request) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.AddTrainingCourse);
        }
    }
}