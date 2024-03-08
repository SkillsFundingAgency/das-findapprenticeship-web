using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.AdditionalQuestion;

[TestFixture]
public class WhenGettingAdditionalQuestionRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid additionalQuestionId,
        GetAdditionalQuestionQueryResult result,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetAdditionalQuestionQuery>(q =>
                    q.ApplicationId == applicationId
                    && q.CandidateId == candidateId
                    && q.AdditionQuestionId == additionalQuestionId)
                , It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var controller = new AdditionalQuestionController(mediator.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            },
            Url = mockUrlHelper.Object,
        };

        var actual = await controller.Get(applicationId, additionalQuestionId) as ViewResult;
        var actualModel = actual?.Model as AddAdditionalQuestionViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.Model.Should().NotBeNull();
            actualModel!.ApplicationId.Should().Be(applicationId);
        }
    }
}