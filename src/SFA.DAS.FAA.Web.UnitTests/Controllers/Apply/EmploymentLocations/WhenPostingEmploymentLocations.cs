using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.EmploymentLocations.Update;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EmploymentLocations
{
    [TestFixture]
    public class WhenPostingEmploymentLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
            bool isEdit,
            Guid candidateId,
            Guid applicationId,
            AddEmploymentLocationsViewModel request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmploymentLocationsController controller)
        {
            //arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator.Setup(x => x.Send(It.Is<UpdateEmploymentLocationsCommand>(c =>
                        c.ApplicationId == applicationId &&
                        c.CandidateId == candidateId &&
                        c.SelectedAddressIds == request.SelectedAddressIds),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            var actual = await controller.AddEmploymentLocations(applicationId, request, isEdit) as RedirectToRouteResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.EmploymentLocationsSummary);
            actual?.RouteValues.Should().NotBeEmpty();
            actual?.RouteValues.Should().ContainKey("ApplicationId");
            actual?.RouteValues.Should().ContainKey("isEdit");
            actual?.RouteValues["ApplicationId"].Should().Be(applicationId);
            actual?.RouteValues["isEdit"].Should().Be(isEdit);
        }
    }
}
