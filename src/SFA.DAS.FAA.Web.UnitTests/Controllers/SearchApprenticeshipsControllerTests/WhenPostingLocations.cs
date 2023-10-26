using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.FAA.Application.Queries.GetGeoPoint;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenPostingLocations
{
    [Test, MoqAutoData]
    public async Task AndModelStateIsInvalid_ModelIsReturned(
        LocationViewModel locationViewModel,
        List<string>? routeIds,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        controller.ModelState.AddModelError("test", "message");

        var result = await controller.Location(routeIds, locationViewModel) as ViewResult;

        result!.Model.Should().BeOfType<LocationViewModel>();
    }

    [Test, MoqInlineAutoData(false, false, null)]
    [MoqInlineAutoData(true, false)]
    public async Task AndCityOrPostcodeIsSelected_AndNoCityOrPostcodeValueInputted_ThenThereIsValidationError(
    bool isValid,
    bool? nationalSearch,
    string? cityOrPostcodeValue,
    LocationViewModel model,
    GetGeoPointQueryResult queryResult,
    Mock<IMediator> mediator)
    {
        model.ErrorDictionary.Clear();
        model.NationalSearch = nationalSearch;
        model.CityOrPostcode = cityOrPostcodeValue;
        mediator.Setup(m => m.Send(It.IsAny<GetGeoPointQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);
        var controller = new Web.Controllers.SearchApprenticeshipsController(mediator.Object);

        var actual = await controller.Location(null, model) as ViewResult;

        actual!.ViewData.ModelState.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.ViewData.ModelState.ErrorCount.Should().Be(1);
            actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
            actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
        }
    }

   [Test, MoqInlineAutoData(false, false)]
   [MoqInlineAutoData(true, false)]
    public async Task AndCityOrPostCodeFilledIn_ButUserDidNotSelectAnAutoSuggestion_ThenThereIsValidationError(
        bool isValid,
        bool? nationalSearch,
        string cityOrPostcodeValue,
        LocationViewModel model,
        GetGeoPointQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator
        )
    {
        model.ErrorDictionary.Clear();
        model.NationalSearch = nationalSearch;
        model.CityOrPostcode = cityOrPostcodeValue;
        if (!isValid)
        {
            mediator.Setup(m => m.Send(It.IsAny<GetGeoPointQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetGeoPointQueryResult() { PostCode = null, Latitude = null, Longitude = null });
        }
        else
        {
            mediator.Setup(m => m.Send(It.IsAny<GetGeoPointQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);
        }
        var controller = new Web.Controllers.SearchApprenticeshipsController(mediator.Object);

        var actual = await controller.Location(null, model) as ViewResult;

        actual!.ViewData.ModelState.IsValid.Should().Be(isValid);
        if (!isValid)
        {
            actual.ViewData.ModelState.ErrorCount.Should().Be(1);
            actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
            actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
        }
    }
}
