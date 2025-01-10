using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;
public class WhenGettingApplicationSubmitted
{
    [Test]
    [MoqInlineAutoData(true, "Do you want to answer equality questions?")]
    [MoqInlineAutoData(false, "Application submitted")]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
        bool hasAnsweredEqualityQuestions,
        string expectedPageTitle,
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplyController controller)
    {
        result.HasAnsweredEqualityQuestions = hasAnsweredEqualityQuestions;
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(c =>
                c.CandidateId.Equals(candidateId)
                && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                new(CustomClaims.CandidateId, candidateId.ToString()),
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var actual = await controller.ApplicationSubmitted(applicationId) as ViewResult;

        actual?.Should().NotBeNull();
        var actualModel = actual!.Model as ApplicationSubmittedViewModel;
        actualModel?.ApplicationId.Should().Be(applicationId);
        actualModel!.PageTitle.Should().Be(expectedPageTitle);
    }
}