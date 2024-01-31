using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    [TestFixture]
    public class WhenPostingSummaryRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
            Guid candidateId,
            UpdateWorkHistoryApplicationCommandResult result,
            [Frozen] Mock<IMediator> mediator)
        {
            //arrange
            var request = new WorkHistorySummaryViewModel
            {
                ApplicationId = Guid.NewGuid(),
                IsSectionCompleted = SectionStatus.Completed,
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

            mediator.Setup(x => x.Send(It.Is<UpdateWorkHistoryApplicationCommand>(c =>
                    c.ApplicationId.Equals(request.ApplicationId)
                    && c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.Summary(request) as RedirectToRouteResult;
            actual.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_With_ValidationError_Is_Called_And_RedirectRoute_Returned(
            Guid candidateId,
            GetApplicationWorkHistoriesQueryResult getApplicationWorkHistoriesQueryResult,
            WorkHistorySummaryViewModel viewModel,
            [Frozen] Mock<IMediator> mediator)
        {
            viewModel.IsSectionCompleted = null;
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

            mediator.Setup(x => x.Send(It.Is<GetApplicationWorkHistoriesQuery>(
                    c => c.ApplicationId.Equals(viewModel.ApplicationId) &&
                         c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationWorkHistoriesQueryResult);

            var actual = await controller.Summary(viewModel) as ViewResult;

            actual.Should().NotBeNull();
            controller.ModelState.Count.Should().BeGreaterThan(0);
            mediator.Verify(x => x.Send(It.IsAny<UpdateWorkHistoryApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
