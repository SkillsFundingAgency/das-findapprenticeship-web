using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

[TestFixture]
public class WhenCallingPostEthnicGroup
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_InValid_Then_Return_View(
        Guid applicationId,
        Guid govIdentifier,
        EqualityQuestionsEthnicGroupViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicGroupViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.EthnicGroup = null;
        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));
            
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicGroupViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("test", "message")]));

        // act
        var actual = await controller.EthnicGroup(validator.Object, applicationId, viewModel) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsEthnicGroupViewModel>();

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.Valid.Should().BeFalse();
            actualModel.ApplicationId.Should().Be(viewModel.ApplicationId);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Cache_Is_InValid_Then_Redirected_To_EqualityFlowGender(
        Guid applicationId,
        Guid govIdentifier,
        EthnicGroup ethnicGroup,
        EqualityQuestionsEthnicGroupViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicGroupViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // assert
        viewModel.EthnicGroup = ethnicGroup.ToString();
        
        var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
        cacheStorageService.Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync((EqualityQuestionsModel?)null);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));
            
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicGroupViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.EthnicGroup(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

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
        EthnicGroup ethnicGroup,
        EqualityQuestionsModel equalityQuestionsModel,
        EqualityQuestionsEthnicGroupViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicGroupViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.EthnicGroup = ethnicGroup.ToString();

        var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
        cacheStorageService.Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(equalityQuestionsModel);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object);
        controller
            .WithUrlHelper(x => x.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://baseUrl"))
            .WithContext(x => x.WithUser(govIdentifier));
            
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicGroupViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var route = RouteNamesHelperService.GetEqualityFlowEthnicSubGroupRoute(ethnicGroup);
        
        // act
        var actual = await controller.EthnicGroup(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(route);
        }
    }
}