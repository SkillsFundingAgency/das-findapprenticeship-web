using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

[TestFixture]
public class WhenCallingPostEthnicSubGroupWhite
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_InValid_Then_Return_View(
        Guid applicationId,
        Guid govIdentifier,
        string modelStateError,
        EqualityQuestionsEthnicSubGroupWhiteViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicSubGroupWhiteViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.EthnicSubGroup = null;

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicSubGroupWhiteViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("test", modelStateError)]));

        // act
        var actual = await controller.EthnicGroupWhite(validator.Object, applicationId, viewModel) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsEthnicSubGroupWhiteViewModel>();

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actual.ViewName.Should().Be("~/Views/apply/EqualityQuestions/EthnicSubGroupWhite.cshtml");
            actualModel.Valid.Should().BeFalse();
            actualModel.ApplicationId.Should().Be(viewModel.ApplicationId);
            controller.ModelState.ContainsKey(nameof(EqualityQuestionsEthnicSubGroupWhiteViewModel)).Should().BeFalse();
            controller.ModelState[nameof(EqualityQuestionsEthnicSubGroupWhiteViewModel)]?.Errors.Should().Contain(e => e.ErrorMessage == modelStateError);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Cache_Is_InValid_Then_Redirected_To_EqualityFlowGender(
        Guid applicationId,
        Guid govIdentifier,
        EthnicSubGroup ethnicSubGroup,
        EqualityQuestionsEthnicSubGroupWhiteViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicSubGroupWhiteViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.EthnicSubGroup = ethnicSubGroup.ToString();
        
        var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync((EqualityQuestionsModel?)null);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicSubGroupWhiteViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.EthnicGroupWhite(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Redirected_To_EthnicSubPage(
        Guid applicationId,
        Guid govIdentifier,
        EthnicSubGroup ethnicSubGroup,
        EqualityQuestionsModel equalityQuestionsModel,
        EqualityQuestionsEthnicSubGroupWhiteViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicSubGroupWhiteViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.EthnicSubGroup = ethnicSubGroup.ToString();
        
        var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
        cacheStorageService
            .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(equalityQuestionsModel);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicSubGroupWhiteViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.EthnicGroupWhite(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary);
        }
    }
}