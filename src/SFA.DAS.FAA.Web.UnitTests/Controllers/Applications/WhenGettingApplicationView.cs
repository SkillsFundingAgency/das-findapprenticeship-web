﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Security.Claims;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications
{
    [TestFixture]
    public class WhenGettingApplicationView
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            Guid applicationId,
            Guid candidateId,
            GetApplicationViewQueryResult queryResult,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplicationsController controller)
        {
            queryResult.IsDisabilityConfident = true;

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            mediator.Setup(x => x.Send(It.Is<GetApplicationViewQuery>(c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var user = new ClaimsPrincipal(new ClaimsIdentity([
                new(CustomClaims.CandidateId, candidateId.ToString())
            ]));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.View(applicationId) as ViewResult;
            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.Model.Should().NotBeNull();
                var actualModel = actual.Model as ApplicationViewModel;
                actualModel!.ApplicationId.Should().Be(applicationId);
                actualModel.WithdrawnDate.Should().Be(queryResult.WithdrawnDate);
                actualModel.MigrationDate.Should().Be(queryResult.MigrationDate);
                actualModel.ShowFoundationTag.Should()
                    .Be(queryResult.ApprenticeshipType == ApprenticeshipTypes.Foundation);
            }
        }
    }
}
