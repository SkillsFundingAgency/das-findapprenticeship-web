using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory;

[TestFixture]
public class WhenGettingAddAJobRequest
{
    [Test, MoqAutoData]
    public void Then_View_Returned(
        Guid applicationId,
        [Frozen] Mock<IMediator> mediator)
    {
        //arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new WorkHistoryController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };

        // act
        var actual = controller.GetAddAJob(applicationId) as ViewResult;

        // assert
        using var scope = new AssertionScope();
        actual.Should().NotBeNull();
        actual?.Model.Should().NotBeNull();

        var actualModel = actual?.Model as AddJobViewModel;
        actualModel?.ApplicationId.Should().Be(applicationId);
    }
}