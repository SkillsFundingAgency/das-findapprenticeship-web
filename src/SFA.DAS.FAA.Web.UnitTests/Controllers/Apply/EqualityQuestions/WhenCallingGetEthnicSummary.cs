using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

public class WhenCallingGetEthnicSummary
{
    private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";

    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Is_Returned(
        Guid applicationId,
        Guid govIdentifier,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        var cacheKey = string.Format($"{Key}", govIdentifier);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync((EqualityQuestionsModel)null!);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));

        // act
        var actual = await controller.Summary(applicationId) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Cached_Value_View_Is_Returned(
        Guid applicationId,
        Guid govIdentifier,
        EqualityQuestionsModel model,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var cacheKey = string.Format($"{Key}", govIdentifier);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(model);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));

        var actual = await controller.Summary(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsSummaryViewModel>();

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(model.ApplicationId);
        }
    }
}