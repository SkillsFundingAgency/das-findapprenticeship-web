using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.UpdateTrainingCourse;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;

public class WhenPostingUpdateTrainingCourseRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
            Guid candidateId,
            EditTrainingCourseViewModel request,
            Mock<IValidator<EditTrainingCourseViewModel>> validator,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.TrainingCoursesController controller)
    {
        // arrange
        request.YearAchieved = "2023";
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<UpdateTrainingCourseCommand>(c =>
                c.TrainingCourseId.Equals(request.TrainingCourseId)
                && c.ApplicationId.Equals(request.ApplicationId)
                && c.CandidateId.Equals(candidateId)
                && c.CourseName.Equals(request.CourseName)
                && c.YearAchieved.Equals(request.YearAchieved)
                ), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EditTrainingCourseViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        // act
        var actual = await controller.PostEdit(validator.Object, request) as RedirectToRouteResult;
        
        // assert
        actual.Should().NotBeNull();
    }
}
