using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.InterviewAdjustments;

public class WhenCallingGet
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        GetInterviewAdjustmentsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        queryResult.Status = null;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetInterviewAdjustmentsQuery>(q => q.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new InterviewAdjustmentsController(mediator.Object)
        {
            Url = mockUrlHelper.Object,
        };
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        // act
        var actual = await controller.Index(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<InterviewAdjustmentsViewModel>();

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Redirect_Route_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        GetInterviewAdjustmentsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetInterviewAdjustmentsQuery>(q => q.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new InterviewAdjustmentsController(mediator.Object)
        {
            Url = mockUrlHelper.Object,
        };
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        // act
        var actual = await controller.Index(applicationId) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary);
        }
    }
}