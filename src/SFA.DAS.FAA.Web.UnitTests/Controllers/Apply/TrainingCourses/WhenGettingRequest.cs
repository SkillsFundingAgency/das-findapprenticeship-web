using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;

public class WhenGettingRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetTrainingCoursesQuery>(q =>
            q.ApplicationId == applicationId
            && q.CandidateId == candidateId)
            , It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTrainingCoursesQueryResult { TrainingCourses = [] });

        var controller = new TrainingCoursesController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        var actual = await controller.Get(applicationId) as ViewResult;
        var actualModel = actual?.Model as TrainingCoursesViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
        }
    }
}