using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.AdditionalQuestion;

[TestFixture]
public class WhenGettingAdditionalQuestionRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid additionalQuestionId,
        int additionalQuestion,
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
                    && q.AdditionalQuestionId == additionalQuestionId
                    && q.AdditionalQuestion == additionalQuestion)
                , It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var controller = new AdditionalQuestionController(mediator.Object) { Url = mockUrlHelper.Object, };
        controller
            .AddControllerContext()
            .WithUser(candidateId);

        var actual = await controller.Get(applicationId, additionalQuestion, additionalQuestionId) as ViewResult;
        var actualModel = actual?.Model as AddAdditionalQuestionViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.Model.Should().NotBeNull();
            actualModel!.ApplicationId.Should().Be(applicationId);
            actualModel!.AdditionalQuestionAnswer.Should().Be(result.Answer);
            actualModel!.AdditionalQuestionLabel.Should().Be(result.QuestionText);
            actualModel!.IsSectionCompleted.Should().Be(result.IsSectionCompleted);
        }
    }
}