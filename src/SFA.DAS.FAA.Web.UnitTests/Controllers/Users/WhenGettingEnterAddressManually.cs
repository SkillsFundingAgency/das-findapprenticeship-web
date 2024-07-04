using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingEnterAddressManually
{
    [Test(Description = "This scenario covers the user opting to manually enter an address from either Enter Postcode or Select Address The back link should return them to the Enter Postcode page.")]
    [MoqInlineAutoData(null, RouteNames.PostcodeAddress, "Create an account", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.PostcodeAddress, "Create an account", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails, "Create an account", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings, "", "Save")]
    public async Task When_Address_Not_From_Lookup_Then_The_BackLink_Returns_Expected(
        UserJourneyPath userJourneyPath,
        string pageBackLink,
        string pageCaption,
        string pageCtaButtonLabel,
        Guid candidateId,
        string? postcode,
        GetCandidateAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.IsAddressFromLookup = false;

        controller.ControllerContext = CreateControllerContext(candidateId);

        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.EnterAddressManually(postcode, userJourneyPath) as ViewResult;

        result.Should().NotBeNull();

        var resultModel = result!.Model as EnterAddressManuallyViewModel;

        resultModel!.BackLink.Should().BeEquivalentTo(pageBackLink);
        resultModel.JourneyPath.Should().Be(userJourneyPath);
        resultModel.PageCaption.Should().Be(pageCaption);
    }

    [Test(Description = "This scenario covers the user opting to manually enter an address from either Enter Postcode or Select Address The back link should return them to the Enter Postcode page."), MoqAutoData]
    public async Task When_Not_Editing_BackLink_Returns_User_To_EnterPostcode(
        Guid candidateId,
        string? postcode,
        GetCandidateAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.IsAddressFromLookup = false;

        controller.ControllerContext = CreateControllerContext(candidateId);

        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.EnterAddressManually(postcode, UserJourneyPath.CreateAccount) as ViewResult;
        result.Should().NotBeNull();
        var resultModel = result!.Model as EnterAddressManuallyViewModel;

        resultModel.Should().NotBeNull();

        resultModel!.BackLink.Should().BeEquivalentTo(RouteNames.PostcodeAddress);
    }

    [Test(Description = "This scenario covers the user coming back from the phone number page having previously selected an address, but then opting to enter their address manually. The back link should return them to the select address page."), MoqAutoData]
    public async Task When_Not_Editing_But_Having_Previously_Selected_An_Address_From_Lookup_Then_BackLink_Returns_User_To_SelectAddress(
        Guid candidateId,
        string? postcode,
        string backLink,
        GetCandidateAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.IsAddressFromLookup = true;

        controller.ControllerContext = CreateControllerContext(candidateId);

        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.EnterAddressManually(postcode) as ViewResult;
        result.Should().NotBeNull();
        var resultModel = result!.Model as EnterAddressManuallyViewModel;

        resultModel.Should().NotBeNull();

        resultModel!.BackLink.Should().BeEquivalentTo(RouteNames.SelectAddress);
    }

    
    [Test(Description = "This scenario covers the user opting to edit an address they have selected from the lookup, but then opting to key their address manually. They should be returned to the address lookup page."), MoqAutoData]
    public async Task When_Entering_An_Address_Manually_From_SelectAddress_Edit_Then_BackLink_Should_Return_User_To_SelectAddress(
        Guid candidateId,
        string keyedPostcode,
        GetCandidateAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.IsAddressFromLookup = true;

        controller.ControllerContext = CreateControllerContext(candidateId);

        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.EnterAddressManually(null, UserJourneyPath.CreateAccount) as ViewResult;

        result.Should().NotBeNull();
        var resultModel = result!.Model as EnterAddressManuallyViewModel;

        resultModel.Should().NotBeNull();
        resultModel!.BackLink.Should().BeEquivalentTo(RouteNames.SelectAddress);
    }

    [Test(Description = "This scenario covers the user opting to edit an address they have selected from the lookup, but then opting to change their postcode and then opting to key their address manually. They should be returned to the postcode page."), MoqAutoData]
    public async Task When_Entering_An_Address_Manually_From_EnterPostcode_Edit_Then_BackLink_Should_Return_User_To_EnterPostcode(
        Guid candidateId,
        string keyedPostcode,
        GetCandidateAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.IsAddressFromLookup = false;

        controller.ControllerContext = CreateControllerContext(candidateId);

        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.EnterAddressManually(null, UserJourneyPath.CreateAccount) as ViewResult;
        result.Should().NotBeNull();
        var resultModel = result!.Model as EnterAddressManuallyViewModel;

        resultModel.Should().NotBeNull();

        resultModel!.BackLink.Should().BeEquivalentTo(RouteNames.PostcodeAddress);
    }

    private ControllerContext CreateControllerContext(Guid candidateId)
    {
        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString())
                }))
            }
        };
    }
}
