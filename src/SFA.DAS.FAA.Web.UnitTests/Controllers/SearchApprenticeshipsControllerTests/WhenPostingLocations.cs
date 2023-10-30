using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetGeoPoint;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
[TestFixture]
public class WhenPostingLocations
{
    [Test, MoqAutoData]
    public async Task AndModelStateIsInvalid_ModelIsReturned(
        LocationViewModel locationViewModel,
        List<string>? routeIds,
        [Greedy] SearchApprenticeshipsController controller)
    {
        controller.ModelState.AddModelError("test", "message");

        var result = await controller.Location(routeIds, locationViewModel) as ViewResult;

        result!.Model.Should().BeOfType<LocationViewModel>();
    }
    
    [Test, MoqAutoData]
    public async Task And_Is_Not_National_Search_And_Null_Location_Then_Validation_Error_Returned(
        LocationViewModel model,
        [Greedy] SearchApprenticeshipsController controller)
    {
        model.NationalSearch = false;
        model.SearchTerm = null;
    
        var actual = await controller.Location(null, model) as ViewResult;
        
        Assert.IsNotNull(actual);
        actual!.ViewData.ModelState.ErrorCount.Should().Be(1);
        actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
        actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
    }
    
    [Test, MoqAutoData]
    public async Task And_Is_Not_National_Search_And_Empty_Location_Then_Validation_Error_Returned(
        LocationViewModel model,
        [Greedy] SearchApprenticeshipsController controller)
    {
        model.NationalSearch = false;
        model.SearchTerm = "";
    
        var actual = await controller.Location(null, model) as ViewResult;
        
        Assert.IsNotNull(actual);
        actual!.ViewData.ModelState.ErrorCount.Should().Be(1);
        actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
        actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
    }
    
    [Test, MoqAutoData]
    public async Task And_Location_Is_Entered_And_Non_National_But_Invalid_Location_Returns_Error(
        string? cityOrPostcodeValue,
        LocationViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        model.ErrorDictionary.Clear();
        model.NationalSearch = false;
        model.SearchTerm = cityOrPostcodeValue;
        mediator.Setup(m => m.Send(It.Is<GetLocationsBySearchQuery>(c => c.SearchTerm.Equals(model.SearchTerm)),
            It.IsAny<CancellationToken>())).ReturnsAsync(new GetLocationsBySearchQueryResult());
        
        var actual = await controller.Location(null, model) as ViewResult;
    
        actual!.ViewData.ModelState.IsValid.Should().BeFalse();
        
        actual.ViewData.ModelState.ErrorCount.Should().Be(1);
        actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
        actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
    
    }
    
}
