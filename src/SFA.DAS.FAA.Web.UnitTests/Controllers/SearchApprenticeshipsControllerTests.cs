using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.GetGeoPoint;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers;

public class SearchApprenticeshipsControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_View_Returned(
        GetSearchApprenticeshipsIndexResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Index() as ViewResult;

        Assert.IsNotNull(actual);
        actual!.Model.Should().BeEquivalentTo((SearchApprenticeshipsViewModel)result);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Browse_By_Interests_View_Returned(
        GetBrowseByInterestsResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetBrowseByInterestsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.BrowseByInterests() as ViewResult;

        Assert.IsNotNull(actual);
        var actualModel = actual!.Model as BrowseByInterestViewModel;
        actualModel.Should().BeEquivalentTo((BrowseByInterestViewModel)result);
    }

    [Test, MoqInlineAutoData(false, false, false, null)]
    //[MoqInlineAutoData(true, false, true)]
    // add this test case back in when the JavaScript can revert the suggestedLocationSelected flag value from true to false
    public async Task AndCityOrPostcodeIsSelected_AndNoCityOrPostcodeValueInputted_ThenThereIsValidationError(
        bool isValid,
        bool? nationalSearch,
        bool suggestedLocationSelected,
        string? cityOrPostcodeValue,
        LocationViewModel model,
        [Greedy] SearchApprenticeshipsController controller)
    {
        model.ErrorDictionary.Clear();
        model.NationalSearch = nationalSearch;
        model.CityOrPostcode = cityOrPostcodeValue;
        model.SuggestedLocationSelected = suggestedLocationSelected;

        var actual = await controller.Location(null, model) as ViewResult;

        actual!.ViewData.ModelState.IsValid.Should().Be(isValid);
        if (isValid == false)
        {
            actual.ViewData.ModelState.ErrorCount.Should().Be(1);
            actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
            actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
        }
    }


    [Test, MoqInlineAutoData(false, false, false)]
    [MoqInlineAutoData(true, false, true)]
    [MoqInlineAutoData(true, false, false)]
    public async Task AndCityOrPostCodeFilledIn_ButUserDidNotSelectAnAutoSuggestion_ThenThereIsValidationError(
        bool isValid,
        bool? nationalSearch,
        bool suggestedLocationSelected,
        string cityOrPostcodeValue,
        LocationViewModel model,
        GetGeoPointQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator
        )
    {
        model.ErrorDictionary.Clear();
        model.NationalSearch = nationalSearch;
        model.CityOrPostcode = cityOrPostcodeValue;
        model.SuggestedLocationSelected = suggestedLocationSelected;
        if (isValid == false)
        {
            mediator.Setup(m => m.Send(It.IsAny<GetGeoPointQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetGeoPointQueryResult() { PostCode = null, Latitude = null, Longitude = null});
        }
        else
        {
            mediator.Setup(m => m.Send(It.IsAny<GetGeoPointQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);
        }
        var controller = new SearchApprenticeshipsController(mediator.Object);
        var actual = await controller.Location(null, model) as ViewResult;

        actual!.ViewData.ModelState.IsValid.Should().Be(isValid);
        if (isValid == false)
        {
            actual.ViewData.ModelState.ErrorCount.Should().Be(1);
            actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
            actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("We don't recognise this city or postcode. Check what you've entered or enter a different location that's nearby");
        }
    }
}