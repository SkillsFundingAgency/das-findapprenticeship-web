using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

[TestFixture]
public class WhenGettingRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        GetQualificationsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        // arrange
        mediator.Setup(x => x.Send(It.Is<GetQualificationsQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new QualificationsController(mediator.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        // act
        var actual = await controller.Get(applicationId) as ViewResult;

        // assert
        using var scope = new AssertionScope();
        actual.Should().NotBeNull();
        actual?.Model.Should().NotBeNull();

        var actualModel = actual?.Model as QualificationsViewModel;
        actualModel?.ApplicationId.Should().Be(applicationId);
        actualModel?.IsSectionCompleted.Should().Be(queryResult.IsSectionCompleted);
        actualModel?.DoYouWantToAddAnyQualifications.Should().Be(
            queryResult.Qualifications.Count == 0 && queryResult.IsSectionCompleted is true ? false : null);
    }
}