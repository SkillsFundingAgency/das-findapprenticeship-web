using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.EmploymentLocations.Update;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EmploymentLocations
{
    [TestFixture]
    public class WhenPostingSummary
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
            bool isEdit,
            Guid candidateId,
            Guid applicationId,
            EmploymentLocationsSummaryViewModel request,
            Mock<IValidator<EmploymentLocationsSummaryViewModel>> validator,
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
                        c.EmploymentLocationSectionStatus == SectionStatus.Completed),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);
            
            validator
                .Setup(x => x.ValidateAsync(It.Is<EmploymentLocationsSummaryViewModel>(m => m == request), CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            // act
            var actual = await controller.Summary(validator.Object, applicationId, request, isEdit) as RedirectToRouteResult;

            // assert
            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.Apply);
            actual?.RouteValues.Should().NotBeEmpty();
            actual?.RouteValues.Should().ContainKey("ApplicationId");
            actual?.RouteValues["ApplicationId"].Should().Be(applicationId);
        }
    }
}
