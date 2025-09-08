using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    [TestFixture]
    public class WhenGettingRequest
    {
        [Test, MoqAutoData]
        public async Task Then_View_Returned(
            Guid applicationId,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator)
        {
            // arrange
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            mediator.Setup(x => x.Send(It.Is<GetJobsQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetJobsQueryResult { Jobs = [] });

            var controller = new WorkHistoryController(mediator.Object)
            {
                Url = mockUrlHelper.Object
            };
            controller
                .AddControllerContext()
                .WithUser(candidateId);

            // act
            var actual = await controller.Get(applicationId) as ViewResult;

            // assert
            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();

            var actualModel = actual?.Model as JobsViewModel;
            actualModel?.ApplicationId.Should().Be(applicationId);
        }
    }
}