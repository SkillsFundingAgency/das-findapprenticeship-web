using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
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

    [Test, MoqInlineAutoData(false, null, null)]
    [MoqInlineAutoData(true, true, false)]
    [MoqInlineAutoData(true, false, true)]
    public void AndNoLocationIsSelected_ThenThereIsValidationError(
        bool isValid,
        bool? cityOrPostcodeSelected,
        bool? allOfEnglandSelected,
        LocationViewModel model,
        [Greedy] SearchApprenticeshipsController controller)
    {
        model.CityOrPostcodeSelected = cityOrPostcodeSelected;
        model.AllOfEnglandSelected = allOfEnglandSelected;

        var actual = controller.Location(null, model) as ViewResult;

        actual!.ViewData.ModelState.IsValid.Should().Be(isValid);
        if (isValid == false)
        {
            actual.ViewData.ModelState.ErrorCount.Should().Be(1);
            actual.ViewData.ModelState["radio-btn"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
            actual.ViewData.ModelState["radio-btn"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Select if you want to enter a city or postcode or if you want to search across all of England");
        }
    }

    [Test, MoqInlineAutoData(false, true, null)]
    [MoqInlineAutoData(true, true)]
    public void AndCityOrPostcodeIsSelected_AndNoCityOrPostcodeValueInputted_ThenThereIsValidationError(
        bool isValid,
        bool? cityOrPostcodeSelected,
        string? cityOrPostcodeValue,
        LocationViewModel model,
        [Greedy] SearchApprenticeshipsController controller)
    {
        model.CityOrPostcodeSelected = cityOrPostcodeSelected;
        model.CityOrPostcode = cityOrPostcodeValue;

        var actual = controller.Location(null, model) as ViewResult;

        actual!.ViewData.ModelState.IsValid.Should().Be(isValid);
        if (isValid == false)
        {
            actual.ViewData.ModelState.ErrorCount.Should().Be(1);
            actual.ViewData.ModelState["CityOrPostcode"]?.ValidationState.Should().Be(Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid);
            actual.ViewData.ModelState["CityOrPostcode"]?.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter a city or postcode");
        }
    }
}