using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions;

[TestFixture]
public class WhenCallingPostEqualityGender
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_InValid_Then_Return_View(
        Guid applicationId,
        Guid govIdentifier,
        EqualityQuestionsGenderViewModel viewModel,
        Mock<IValidator<EqualityQuestionsGenderViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.IsGenderIdentifySameSexAtBirth = null;
        viewModel.Sex = null;

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
        };
        controller
            .AddControllerContext()
            .WithUser(govIdentifier);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsGenderViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("test", "message")]));

        // act
        var actual = await controller.Gender(validator.Object, applicationId, viewModel) as ViewResult;
        var actualModel = actual!.Model.As<EqualityQuestionsGenderViewModel>();

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
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_EqualityEthnicGroup(
        Guid applicationId,
        Guid govIdentifier,
        GenderIdentity gender,
        EqualityQuestionsGenderViewModel viewModel,
        Mock<IValidator<EqualityQuestionsGenderViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        // arrange
        viewModel.Sex = gender.ToString();
        viewModel.IsEdit = false;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
        };
        controller
            .AddControllerContext()
            .WithUser(govIdentifier);
        validator
            .Setup(x => x.ValidateAsync(It.Is<EqualityQuestionsGenderViewModel>(m => m == viewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Gender(validator.Object, applicationId, viewModel) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.RouteName.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup);
        }
    }
}