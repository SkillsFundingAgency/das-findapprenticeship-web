using MediatR;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers.Apply;
using Microsoft.AspNetCore.Http;
using SFA.DAS.FAA.Web.AppStart;
using System.Security.Claims;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.SkillsAndStrengths;

public class WhenCallingGet
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Is_Called_And_View_Returned(
        Guid candidateId,
        Guid applicationId,
        GetExpectedSkillsAndStrengthsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        var controller = new SkillsAndStrengthsController(mediator.Object)
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

        mediator.Setup(x => x.Send(It.Is<GetExpectedSkillsAndStrengthsQuery>
            (x => x.ApplicationId == applicationId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.Get(applicationId);
        var actualViewResult = actual as ViewResult;

        using (new AssertionScope())
        {
            actual.Should().BeOfType<ViewResult>();
            actualViewResult.Model.Should().BeOfType<SkillsAndStrengthsViewModel>();
            actualViewResult.Model.As<SkillsAndStrengthsViewModel>().Employer.Should().BeEquivalentTo(queryResult.Employer);
            actualViewResult.Model.As<SkillsAndStrengthsViewModel>().ExpectedSkillsAndStrengths.Should().BeEquivalentTo(queryResult.ExpectedSkillsAndStrengths.ToList());
            actualViewResult.Model.As<SkillsAndStrengthsViewModel>().SkillsAndStrengths.Should().BeEquivalentTo(queryResult.Strengths);
        }
    }
}