using System.Security.Claims;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.InterviewAdjustments;
public class WhenCallingGet
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        GetInterviewAdjustmentsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetInterviewAdjustmentsQuery>(q => q.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new InterviewAdjustmentsController(mediator.Object)
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

        var actual = await controller.Index(applicationId) as ViewResult;
        var actualModel = actual.Model.As<InterviewAdjustmentsViewModel>();

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
        }
    }
}
