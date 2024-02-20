using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.UpdateTrainingCourse;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenPostingUpdateTrainingCourseRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
            Guid candidateId,
            EditTrainingCourseViewModel request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.TrainingCoursesController controller)
    {
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

        var actual = await controller.PostEdit(request) as RedirectToRouteResult;
        actual.Should().NotBeNull();
    }
}
