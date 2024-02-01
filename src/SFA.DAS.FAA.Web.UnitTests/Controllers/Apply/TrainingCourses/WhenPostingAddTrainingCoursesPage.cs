using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenPostingAddTrainingCoursesPage
{
    [Test, MoqAutoData]
    public async Task And_Validation_Error_Then_View_Returned(
        AddTrainingCourseViewModel viewModel, 
        [Frozen] Mock<IMediator> mediator)
    {
        viewModel.AddTrainingCourse = null;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        var controller = new TrainingCoursesController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };

        var actual = await controller.Post(viewModel) as ViewResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            controller.ModelState.Count.Should().BeGreaterThan(0);
            mediator.Verify(x => x.Send(It.IsAny<UpdateTrainingCoursesApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Test, MoqAutoData]
    public async Task And_User_Has_No_Training_Courses_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        UpdateTrainingCoursesApplicationCommandResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        var request = new AddTrainingCourseViewModel
        {
            ApplicationId = applicationId,
            AddTrainingCourse = "No",
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
        var request = new AddTrainingCourseViewModel
        {
            ApplicationId = applicationId,
            AddTrainingCourse = "Yes",
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
            actual?.RouteName.Should().Be("/");
        }
    }
}
