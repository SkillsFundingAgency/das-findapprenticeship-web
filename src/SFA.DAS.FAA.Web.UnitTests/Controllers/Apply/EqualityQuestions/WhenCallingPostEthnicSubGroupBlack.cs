using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

[TestFixture]
public class WhenCallingPostEthnicSubGroupBlack
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_InValid_Then_Return_View(
        Guid applicationId,
        Guid govIdentifier,
        string modelStateError,
        EqualityQuestionsEthnicSubGroupBlackViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicSubGroupBlackViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.EthnicSubGroup = null;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, govIdentifier.ToString()) }))
                }
            }
        };
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicSubGroupBlackViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("test", modelStateError)]));

        // act
        var actual = await controller.EthnicGroupBlack(validator.Object, applicationId, viewModel) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsEthnicSubGroupBlackViewModel>();

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actual.ViewName.Should().Be("~/Views/apply/EqualityQuestions/EthnicSubGroupBlack.cshtml");
            actualModel.Valid.Should().BeFalse();
            actualModel.ApplicationId.Should().Be(viewModel.ApplicationId);
            controller.ModelState.ContainsKey(nameof(EqualityQuestionsEthnicSubGroupBlackViewModel)).Should().BeFalse();
            controller.ModelState[nameof(EqualityQuestionsEthnicSubGroupBlackViewModel)]?.Errors.Should().Contain(e => e.ErrorMessage == modelStateError);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Cache_Is_InValid_Then_Redirected_To_EqualityFlowGender(
        Guid applicationId,
        Guid govIdentifier,
        EthnicSubGroup ethnicSubGroup,
        EqualityQuestionsEthnicSubGroupBlackViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicSubGroupBlackViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
        viewModel.EthnicSubGroup = ethnicSubGroup.ToString();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        cacheStorageService.Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync((EqualityQuestionsModel?)null);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, govIdentifier.ToString()) }))
                }
            }
        };
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicSubGroupBlackViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.EthnicGroupBlack(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

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
        EqualityQuestionsEthnicSubGroupBlackViewModel viewModel,
        Mock<IValidator<EqualityQuestionsEthnicSubGroupBlackViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
        viewModel.EthnicSubGroup = ethnicSubGroup.ToString();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        cacheStorageService.Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
            .ReturnsAsync(equalityQuestionsModel);

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, govIdentifier.ToString()) }))
                }
            }
        };
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsEthnicSubGroupBlackViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.EthnicGroupBlack(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary);
        }
    }
}