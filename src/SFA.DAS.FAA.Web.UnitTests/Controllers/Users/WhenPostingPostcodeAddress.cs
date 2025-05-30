﻿using CreateAccount.GetCandidatePostcodeAddress;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenPostingPostcodeAddress
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings)]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Enter_Your_Address(
        UserJourneyPath journeyPath,
        string redirectRoute,
        string govIdentifier,
        string email,
        GetCandidatePostcodeAddressQueryResult queryResult,
        PostcodeAddressViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.PostcodeExists = true;
        model.JourneyPath = journeyPath;

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.IsAny<GetCandidatePostcodeAddressQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.PostcodeAddress(model) as RedirectToRouteResult;

        result.Should().NotBeNull();
        result!.RouteName.Should().Be(RouteNames.SelectAddress);
        result.RouteValues["journeyPath"].Should().Be(journeyPath);
        mediator.Verify(x => x.Send(It.Is<GetCandidatePostcodeAddressQuery>(c =>
            c.Postcode.Equals(model.Postcode)
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        PostcodeAddressViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ModelState.AddModelError("SomeProperty", "SomeError");

        var result = await controller.PostcodeAddress(model) as ViewResult;

        result.Should().NotBeNull();
        result!.Model.Should().Be(model);
    }

    [Test, MoqAutoData]
    public async Task And_Mediator_Throws_Exception_Then_InvalidOperationException(
        string govIdentifier,
        string email,
        PostcodeAddressViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                    }))

            }
        };
        mediator.Setup(x => x.Send(It.IsAny<GetCandidatePostcodeAddressQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var result = await controller.PostcodeAddress(model) as ViewResult;

        result.Should().NotBeNull();
        result!.Model.Should().Be(model);
        controller.ModelState.Count.Should().BeGreaterThan(0);
    }

    [Test, MoqAutoData]
    public async Task And_Mediator_Returns_InvalidPostcode_Then_Add_Model_Error(
        string govIdentifier,
        string email,
        GetCandidatePostcodeAddressQueryResult queryResult,
        PostcodeAddressViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        queryResult.PostcodeExists = false;

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                    }))

            }
        };

        mediator.Setup(x => x.Send(It.IsAny<GetCandidatePostcodeAddressQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.PostcodeAddress(model) as ViewResult;

        result.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.Is<GetCandidatePostcodeAddressQuery>(c =>
            c.Postcode.Equals(model.Postcode)
            ), It.IsAny<CancellationToken>()), Times.Once);
        controller.ModelState.Count.Should().BeGreaterThan(0);
    }
}
