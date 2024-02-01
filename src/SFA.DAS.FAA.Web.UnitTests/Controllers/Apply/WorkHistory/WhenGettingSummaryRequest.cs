using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory;

[TestFixture]
public class WhenGettingSummaryRequest
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
        Guid applicationId,
        Guid candidateId,
        GetJobsQueryResult result,
        [Frozen] Mock<IMediator> mediator)
    {
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

        mediator.Setup(x => x.Send(It.Is<GetJobsQuery>(
                c => c.ApplicationId.Equals(applicationId) &&
                     c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Summary(applicationId) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual?.Model as WorkHistorySummaryViewModel;
        var workHistories = actualModel?.WorkHistories as List<WorkHistoryViewModel>;

        actualModel?.ApplicationId.Should().Be(applicationId);
        actualModel?.WorkHistories.Should().HaveCountGreaterOrEqualTo(result.Jobs.Count);
        workHistories.Should().BeEquivalentTo(result.Jobs, options => options
            .Excluding(x => x.JobTitle)
            .Excluding(x => x.StartDate)
            .Excluding(x => x.EndDate)
            .Excluding(x => x.ApplicationId));
    }
}