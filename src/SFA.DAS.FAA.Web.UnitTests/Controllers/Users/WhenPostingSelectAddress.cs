﻿using CreateAccount.GetAddressesByPostcode;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.SelectedAddress;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenPostingSelectAddress
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.PhoneNumber)]
    [MoqInlineAutoData(UserJourneyPath.PhoneNumber, RouteNames.PhoneNumber)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings)]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Phone_Number_Page(
        UserJourneyPath journeyPath,
        string redirectRoute,
        string govIdentifier,
        string email,
        Guid candidateId,
        GetAddressesByPostcodeQueryResult addressesByPostcodeQueryResult,
        SelectAddressViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        model.JourneyPath = journeyPath;
        model.SelectedAddress = addressesByPostcodeQueryResult.Addresses.First().Uprn;

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

            }
        };

        mediator.Setup(x => x.Send(It.IsAny<GetAddressesByPostcodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(addressesByPostcodeQueryResult);

        var result = await controller.SelectAddress(model) as RedirectToRouteResult;

        result.Should().NotBeNull();
        result!.RouteName.Should().Be(redirectRoute);
        mediator.Verify(x => x.Send(It.IsAny<UpdateAddressCommand>(), CancellationToken.None
            ), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        GetAddressesByPostcodeQueryResult addressesByPostcodeQueryResult,
        SelectAddressViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        model.SelectedAddress = "1";

        controller.ModelState.AddModelError("SomeProperty", "SomeError");

        mediator.Setup(x => x.Send(It.IsAny<GetAddressesByPostcodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(addressesByPostcodeQueryResult);

        var result = await controller.SelectAddress(model) as ViewResult;

        result.Should().NotBeNull();
        result.Model.As<SelectAddressViewModel>().Addresses.Should()
            .BeEquivalentTo(addressesByPostcodeQueryResult.Addresses.Select(x => (AddressViewModel)x).ToList());
    }
}
