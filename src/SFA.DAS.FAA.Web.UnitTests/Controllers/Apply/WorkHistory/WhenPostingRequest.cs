using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Web.AppStart;
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
            var request = new AddWorkHistoryViewModel
            {
                ApplicationId = Guid.NewGuid(),
                AddJob = "Yes",
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
            var request = new AddWorkHistoryViewModel
            {
                ApplicationId = Guid.NewGuid(),
                AddJob = "No",
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
        public async Task Then_The_Mediator_Command_With_ValidationError_Is_Called_And_RedirectRoute_Returned(
            UpdateWorkHistoryApplicationCommandResult result,
            AddWorkHistoryViewModel viewModel,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            viewModel.AddJob = null;
            controller.Url = Mock.Of<IUrlHelper>();

            var actual = await controller.Post(viewModel) as ViewResult;
            
            actual.Should().NotBeNull();
            controller.ModelState.Count.Should().BeGreaterThan(0);
            mediator.Verify(x => x.Send(It.IsAny<UpdateWorkHistoryApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
