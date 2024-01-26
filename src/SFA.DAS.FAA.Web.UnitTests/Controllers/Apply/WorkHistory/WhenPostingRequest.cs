using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
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
            UpdateApplicationCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            //arrange
            var request = new AddWorkHistoryRequest
            {
                ApplicationId = Guid.NewGuid(),
                AddJob = "Yes",
            };

            mediator.Setup(x => x.Send(It.IsAny<UpdateApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Post(request) as RedirectToRouteResult;
            actual.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned_TaskList(
            UpdateApplicationCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            //arrange
            var request = new AddWorkHistoryRequest
            {
                ApplicationId = Guid.NewGuid(),
                AddJob = "No",
            };

            mediator.Setup(x => x.Send(It.IsAny<UpdateApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Post(request) as RedirectToRouteResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.Apply);
            actual?.RouteValues.Should().NotBeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_With_ValidationError_Is_Called_And_RedirectRoute_Returned(
            UpdateApplicationCommandResult result,
            AddWorkHistoryRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.Apply.WorkHistoryController controller)
        {
            controller.ModelState.AddModelError("SomeProperty", "SomeError");

            mediator.Setup(x => x.Send(It.IsAny<UpdateApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Post(request) as ViewResult;
            actual.Should().BeNull();
            controller.ModelState.Count.Should().BeGreaterThan(0);
        }
    }
}
