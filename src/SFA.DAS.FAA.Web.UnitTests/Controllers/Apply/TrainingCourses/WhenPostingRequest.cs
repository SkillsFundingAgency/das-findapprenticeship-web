using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenPostingRequest
{
    [Test, MoqAutoData]
    public async Task And_User_Has_No_Training_Courses_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        UpdateTrainingCoursesApplicationCommandResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        var request = new TrainingCoursesViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyTrainingCourses = false,
        };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        mediator.Setup(x => x.Send(It.IsAny<UpdateTrainingCoursesApplicationCommandResult>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Post(request) as RedirectToRouteResult;

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
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        var request = new TrainingCoursesViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyTrainingCourses = true,
        };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        mediator.Setup(x => x.Send(It.IsAny<UpdateTrainingCoursesApplicationCommandResult>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Post(request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.AddTrainingCourse);
        }
    }
}
