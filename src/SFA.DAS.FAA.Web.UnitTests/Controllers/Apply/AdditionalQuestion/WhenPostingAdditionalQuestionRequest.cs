using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.AdditionalQuestion;

[TestFixture]
public class WhenPostingAdditionalQuestionRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid additionalQuestionId,
        int additionalQuestion,
        AddAdditionalQuestionViewModel model,
        AddAdditionalQuestionCommandHandlerResult result,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<AddAdditionalQuestionCommand>(c =>
                    c.ApplicationId == applicationId
                    && c.CandidateId == candidateId
                    && c.Id == additionalQuestionId
                    && c.Answer == model.AdditionalQuestionAnswer
                    && c.UpdatedAdditionalQuestion == additionalQuestion
                    && c.AdditionalQuestionSectionStatus == SectionStatus.Completed)
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

        var actual = await controller.Post(applicationId, additionalQuestion, additionalQuestionId, model) as RedirectToRouteResult;
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().Be(RouteNames.Apply);
        }
    }
}