using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;

public class WhenGettingAddACourseRequest
{
    [Test, MoqAutoData]
    public void Then_View_Returned(
    Guid applicationId,
    [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        var controller = new TrainingCoursesController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };

        // act
        var actual = controller.GetAddATrainingCourse(applicationId) as ViewResult;
        var actualModel = actual?.Model as AddJobViewModel;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();
            actualModel?.ApplicationId.Should().Be(applicationId);
        }
    }
}