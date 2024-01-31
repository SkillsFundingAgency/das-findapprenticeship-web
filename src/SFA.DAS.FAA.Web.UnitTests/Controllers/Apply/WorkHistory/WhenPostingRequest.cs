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
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    [TestFixture]
    public class WhenPostingRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
            Guid candidateId,
            UpdateWorkHistoryApplicationCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            //arrange
            var request = new JobsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                DoYouWantToAddAnyJobs = true
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator.Setup(x => x.Send(It.Is<UpdateWorkHistoryApplicationCommand>(c=>
                    c.ApplicationId.Equals(request.ApplicationId)
                    && c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Post(request) as RedirectToRouteResult;
            actual.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned_TaskList(
            Guid candidateId,
            UpdateWorkHistoryApplicationCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            //arrange
            var request = new JobsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                DoYouWantToAddAnyJobs = false
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };
            mediator.Setup(x => x.Send(It.IsAny<UpdateWorkHistoryApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Post(request) as RedirectToRouteResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.Apply);
            actual?.RouteValues.Should().NotBeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Then_When_Section_Is_Completed_The_Section_Status_Is_Updated_RedirectRoute_Returned(
            Guid candidateId,
            UpdateWorkHistoryApplicationCommandResult result,
            [Frozen] Mock<IMediator> mediator)
        {
            //arrange
            var request = new JobsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                ShowJobHistory = true,
                IsSectionCompleted = true
            };
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");
            var controller = new WorkHistoryController(mediator.Object)
            {
                Url = mockUrlHelper.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new(CustomClaims.CandidateId, candidateId.ToString()),
                        }))
                    }
                }
            };

            mediator.Setup(x => x.Send(It.Is<UpdateApplicationCommand>(c =>
                    c.ApplicationId.Equals(request.ApplicationId)
                    && c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Post(request) as RedirectToRouteResult;
            actual.Should().NotBeNull();
        }
    }
}
