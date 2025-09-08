using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

[TestFixture]
public class WhenCallingGetEthnicSubGroupBlack
{
    private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";

    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        Guid applicationId,
        Guid govIdentifier,
        EqualityQuestionsModel model,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var cacheKey = string.Format($"{Key}", govIdentifier);
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(model);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object) { Url = mockUrlHelper.Object, };
        controller
            .AddControllerContext()
            .WithUser(govIdentifier);

        var actual = await controller.EthnicGroupBlack(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsEthnicSubGroupBlackViewModel>();

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(model.ApplicationId);
        }
    }
}