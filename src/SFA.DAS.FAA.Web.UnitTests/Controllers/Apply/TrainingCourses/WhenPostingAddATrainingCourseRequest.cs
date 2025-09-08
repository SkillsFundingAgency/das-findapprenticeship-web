using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.AddTrainingCourse;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;

public class WhenPostingAddATrainingCourseRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
    Guid candidateId,
    int yearAchieved,
    AddTrainingCourseViewModel request,
    AddTrainingCourseCommandResponse result,
    Mock<IValidator<AddTrainingCourseViewModel>> validator,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] TrainingCoursesController controller)
    {
        // arrange
        request.YearAchieved = yearAchieved.ToString();
        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<AddTrainingCourseCommand>(c =>
        c.ApplicationId == request.ApplicationId
        && c.CandidateId == candidateId
        && c.CourseName == request.CourseName
        && c.YearAchieved == int.Parse(request.YearAchieved)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<AddTrainingCourseViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.PostAddATrainingCourse(validator.Object, request) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.TrainingCourses);
        }
    }
}
