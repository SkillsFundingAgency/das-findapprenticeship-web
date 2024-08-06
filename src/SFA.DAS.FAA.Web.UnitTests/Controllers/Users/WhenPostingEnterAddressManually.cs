using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.CreateAccount.ManuallyEnteredAddress;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenPostingEnterAddressManually
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.PhoneNumber)]
    [MoqInlineAutoData(UserJourneyPath.PhoneNumber, RouteNames.PhoneNumber)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.AccountFound, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings)]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Phone_Number_Page(
        UserJourneyPath journeyPath,
        string redirectRoute,
        string govIdentifier,
        string email,
        Guid candidateId,
        EnterAddressManuallyViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        model.JourneyPath = journeyPath;
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

        var result = await controller.EnterAddressManually(model) as RedirectToRouteResult;

        result.Should().NotBeNull();
        result!.RouteName.Should().Be(redirectRoute);
        mediator.Verify(x => x.Send(It.IsAny<UpdateManuallyEnteredAddressCommand>(), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        EnterAddressManuallyViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ModelState.AddModelError("SomeProperty", "SomeError");

        var result = await controller.EnterAddressManually(model) as ViewResult;

        result.Should().NotBeNull();
    }
}
