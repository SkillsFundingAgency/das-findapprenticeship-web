using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Constants;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

[TestFixture]
public class WhenPostingLocations
{
    [Test, MoqAutoData]
    public async Task AndModelStateIsInvalid_ModelIsReturned(
        LocationViewModel locationViewModel,
        List<string>? routeIds,
        Mock<IValidator<LocationViewModel>> validator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        // arrange
        validator
            .Setup(x => x.ValidateAsync(It.Is<LocationViewModel>(m => m == locationViewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("test", "message")]));

        // act
        var result = await controller.Location(validator.Object, routeIds, locationViewModel) as ViewResult;

        // assert
        result!.Model.Should().BeOfType<LocationViewModel>();
    }
    
    [Test, MoqAutoData]
    public async Task And_Is_Not_National_Search_And_Null_Location_Then_Validation_Error_Returned(
        LocationViewModel model,
        Mock<IValidator<LocationViewModel>> validator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        // arrange
        model.NationalSearch = false;
        model.SearchTerm = null;
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<LocationViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Location(validator.Object, null, model) as ViewResult;

        // assert
        Assert.That(actual, Is.Not.Null);
        actual!.ViewData.ModelState.ErrorCount.Should().Be(1);
        actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
        actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
    }
    
    [Test, MoqAutoData]
    public async Task And_Is_Not_National_Search_And_Empty_Location_Then_Validation_Error_Returned(
        LocationViewModel model,
        Mock<IValidator<LocationViewModel>> validator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        // arrange
        model.NationalSearch = false;
        model.SearchTerm = "";
        validator
            .Setup(x => x.ValidateAsync(It.Is<LocationViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Location(validator.Object, null, model) as ViewResult;

        // assert
        Assert.That(actual, Is.Not.Null);
        actual!.ViewData.ModelState.ErrorCount.Should().Be(1);
        actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
        actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
    }
    
    [Test, MoqAutoData]
    public async Task And_Location_Is_Entered_And_Non_National_But_Invalid_Location_Returns_Error(
        string? cityOrPostcodeValue,
        LocationViewModel model,
        Mock<IValidator<LocationViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        // arrange
        model.NationalSearch = false;
        model.SearchTerm = cityOrPostcodeValue;
        mediator.Setup(m => m.Send(It.Is<GetBrowseByInterestsLocationQuery>(c => c.LocationSearchTerm.Equals(model.SearchTerm)),
            It.IsAny<CancellationToken>())).ReturnsAsync(new GetBrowseByInterestsLocationQueryResult());
        validator
            .Setup(x => x.ValidateAsync(It.Is<LocationViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Location(validator.Object, null, model) as ViewResult;

        // assert
        actual!.ViewData.ModelState.IsValid.Should().BeFalse();
        actual.ViewData.ModelState.ErrorCount.Should().Be(1);
        actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
        actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a valid city or postcode");
        mediator.Verify(x=>x.Send(It.IsAny<GetBrowseByInterestsLocationQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Redirect_Result_Is_Returned(
        LocationViewModel locationViewModel,
        List<string>? routeIds,
        Mock<IValidator<LocationViewModel>> validator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        // arrange
        validator
            .Setup(x => x.ValidateAsync(It.Is<LocationViewModel>(m => m == locationViewModel), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.Location(validator.Object, routeIds, locationViewModel) as RedirectToRouteResult;

        // assert
        result!.RouteName.Should().Be(RouteNames.SearchResults);
        result.RouteValues!["RoutePath"].Should().Be(Constants.SearchResultRoutePath.Location);
    }
}
