using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.DisabilityConfident;

[TestFixture]
public class WhenCallingSummaryGet
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        GetDisabilityConfidentDetailsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        mediator.Setup(x => x.Send(It.Is<GetDisabilityConfidentDetailsQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new DisabilityConfidentController(mediator.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        // act
        var actual = await controller.GetSummary(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<DisabilityConfidentSummaryViewModel>();

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
        }
    }
}