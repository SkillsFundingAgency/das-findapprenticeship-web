using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Application
{
    [TestFixture]
    public class WhenGettingApplicationPreview
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            Guid applicationId,
            Guid candidateId,
            GetApplicationSummaryQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplyController controller)
        {
            
            queryResult.IsDisabilityConfident = true;
            queryResult.IsApplicationComplete = true;
            
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            mediator.Setup(x => x.Send(It.Is<GetApplicationSummaryQuery>(c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString()),
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Preview(applicationId) as ViewResult;
            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.Model.Should().NotBeNull();
                var actualModel = actual.Model as ApplicationSummaryViewModel;
                actualModel!.ApplicationId.Should().Be(applicationId);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_Redirect_Is_Returned(
            Guid applicationId,
            Guid candidateId,
            GetApplicationSummaryQueryResult queryResult,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplyController controller)
        {
            queryResult.EducationHistory.TrainingCoursesStatus = SectionStatus.InProgress;
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            mediator.Setup(x => x.Send(It.Is<GetApplicationSummaryQuery>(c =>
                c.ApplicationId.Equals(applicationId) && 
                c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString()),
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Preview(applicationId) as RedirectToRouteResult;
            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.RouteName.Should().NotBeNull();
                actual!.RouteName.Should().Be((RouteNames.Apply));
            }
        }
    }
}
