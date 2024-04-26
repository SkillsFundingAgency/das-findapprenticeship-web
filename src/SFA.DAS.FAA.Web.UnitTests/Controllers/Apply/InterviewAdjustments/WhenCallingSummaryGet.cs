using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.InterviewAdjustments;

[TestFixture]
public class WhenCallingSummaryGet
{
    [Test]
    [MoqInlineAutoData(true, "some text")]
    [MoqInlineAutoData(false, "")]
    public async Task Then_View_Is_Returned(
        bool isSupportRequestRequired,
        string interviewAdjustmentsDescription,
        Guid applicationId,
        Guid candidateId,
        GetInterviewAdjustmentsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        queryResult.InterviewAdjustmentsDescription = interviewAdjustmentsDescription;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetInterviewAdjustmentsQuery>(q => q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new InterviewAdjustmentsController(mediator.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            }
        };

        var actual = await controller.GetSummary(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<InterviewAdjustmentSummaryViewModel>();

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
            actualModel.IsSectionCompleted.Should().Be(queryResult.Status);
            actualModel.SupportRequestAnswer.Should().Be(queryResult.InterviewAdjustmentsDescription);
            actualModel.IsSupportRequestRequired.Should().Be(isSupportRequestRequired);
        }
    }
}