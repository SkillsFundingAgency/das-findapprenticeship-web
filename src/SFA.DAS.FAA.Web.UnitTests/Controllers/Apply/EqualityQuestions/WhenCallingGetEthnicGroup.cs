using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

[TestFixture]
public class WhenCallingGetEthnicGroup
{
    private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";

    [Test, MoqAutoData]
    public async Task Then_Cached_Value_View_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        EqualityQuestionsModel model,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var cacheKey = string.Format($"{Key}", candidateId);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(model);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        var actual = await controller.EthnicGroup(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsEthnicGroupViewModel>();

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(model.ApplicationId);
        }
    }
}