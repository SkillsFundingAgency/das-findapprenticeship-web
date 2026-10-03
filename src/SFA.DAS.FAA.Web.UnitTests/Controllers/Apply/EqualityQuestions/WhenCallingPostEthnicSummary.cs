using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Commands.EqualityQuestions;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

public class WhenCallingPostEthnicSummary
{
    private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";

    [Test, MoqAutoData]
    public async Task Then_Redirect_Route_Is_Returned(
        Guid applicationId,
        Guid govIdentifier,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var cacheKey = string.Format($"{Key}", govIdentifier);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync((EqualityQuestionsModel)null!);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));

        var actual = await controller.Summary(applicationId, new EqualityQuestionsSummaryViewModel()) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Cached_Value_Redirect_Route_Is_Returned(
        Guid applicationId,
        Guid govIdentifier,
        Guid candidateId,
        EqualityQuestionsModel model,
        CreateEqualityQuestionsCommandResult response,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var cacheKey = string.Format($"{Key}", govIdentifier);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(model);

        mediator.Setup(x => x.Send(It.Is<CreateEqualityQuestionsCommand>(c =>
                c.CandidateId.Equals(candidateId)
                && c.EthnicGroup.Equals(model.EthnicGroup)
                && c.Sex.Equals(model.Sex)
                && c.EthnicSubGroup.Equals(model.EthnicSubGroup)
                && c.IsGenderIdentifySameSexAtBirth!.Equals(model.IsGenderIdentifySameSexAtBirth)
                && c.OtherEthnicSubGroupAnswer!.Equals(model.OtherEthnicSubGroupAnswer)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId).WithClaim(ClaimTypes.NameIdentifier, govIdentifier.ToString()));

        var actual = await controller.Summary(applicationId, new EqualityQuestionsSummaryViewModel()) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
            actual!.RouteValues!["showEqualityQuestionsBanner"].Should().Be(true);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_When_EthnicGroup_Is_PreferNotToSay_EthnicSubGroup_Is_Null_In_Command(
        Guid applicationId,
        Guid candidateId,
        EqualityQuestionsModel model,
        CreateEqualityQuestionsCommandResult response,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        model.EthnicGroup = EthnicGroup.PreferNotToSay;
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(It.IsAny<string>()))
            .ReturnsAsync(model);

        mediator.Setup(x => x.Send(It.IsAny<CreateEqualityQuestionsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        await controller.Summary(applicationId, new EqualityQuestionsSummaryViewModel());

        mediator.Verify(x => x.Send(
            It.Is<CreateEqualityQuestionsCommand>(c =>
                c.EthnicGroup == EthnicGroup.PreferNotToSay
                && c.EthnicSubGroup == null),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_When_EthnicGroup_Is_Not_PreferNotToSay_EthnicSubGroup_Is_Passed_In_Command(
        Guid applicationId,
        Guid candidateId,
        EqualityQuestionsModel model,
        CreateEqualityQuestionsCommandResult response,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        model.EthnicGroup = EthnicGroup.White;
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(It.IsAny<string>()))
            .ReturnsAsync(model);

        mediator.Setup(x => x.Send(It.IsAny<CreateEqualityQuestionsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(candidateId));

        await controller.Summary(applicationId, new EqualityQuestionsSummaryViewModel());

        mediator.Verify(x => x.Send(
            It.Is<CreateEqualityQuestionsCommand>(c =>
                c.EthnicGroup == EthnicGroup.White
                && c.EthnicSubGroup == model.EthnicSubGroup),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}