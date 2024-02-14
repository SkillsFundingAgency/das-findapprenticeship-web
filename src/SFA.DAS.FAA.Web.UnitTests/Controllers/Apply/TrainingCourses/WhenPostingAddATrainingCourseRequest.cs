using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.AddTrainingCourse;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenPostingAddATrainingCourseRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
    Guid candidateId,
    int yearAchieved,
    AddTrainingCourseViewModel request,
    AddTrainingCourseCommandResponse result,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] Web.Controllers.Apply.TrainingCoursesController controller)
    {
        request.YearAchieved = yearAchieved.ToString();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<AddTrainingCourseCommand>(c =>
        c.ApplicationId == request.ApplicationId
        && c.CandidateId == candidateId
        && c.CourseName == request.CourseName
        && c.YearAchieved == int.Parse(request.YearAchieved)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.PostAddATrainingCourse(request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.TrainingCourses);
        }
    }
}
