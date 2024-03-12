using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications
{
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
            //arrange
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            mediator.Setup(x => x.Send(It.Is<GetQualificationsQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controller = new QualificationsController(mediator.Object)
            {
                Url = mockUrlHelper.Object
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            var actual = await controller.Get(applicationId) as ViewResult;

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
}