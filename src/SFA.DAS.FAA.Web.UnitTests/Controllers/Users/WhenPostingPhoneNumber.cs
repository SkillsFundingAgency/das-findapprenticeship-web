using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.PhoneNumber;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenPostingPhoneNumber
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.NotificationPreferences)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings)]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Enter_Your_Address(
        UserJourneyPath journeyPath,
        string redirectRoute,
        string govIdentifier,
        string email,
        string phone,
        Guid candidateId,
        PhoneNumberViewModel model,
        Mock<IValidator<PhoneNumberViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        model.JourneyPath = journeyPath;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.MobilePhone, phone),
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))
            }
        };
        validator
            .Setup(x => x.ValidateAsync(It.Is<PhoneNumberViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.PhoneNumber(validator.Object, model) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        result.RouteName.Should().Be(redirectRoute);
        result.RouteValues["journeyPath"].Should().Be(journeyPath);
        mediator.Verify(x => x.Send(It.Is<UpdatePhoneNumberCommand>(c =>
            c.Email.Equals(email)
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        string govIdentifier,
        string email,
        string phone,
        PhoneNumberViewModel model,
        Mock<IValidator<PhoneNumberViewModel>> validator,
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
                        new Claim(ClaimTypes.MobilePhone, phone)
                    }))

            }
        };
        validator
            .Setup(x => x.ValidateAsync(It.Is<PhoneNumberViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("SomeProperty", "SomeError")]));

        var result = await controller.PhoneNumber(validator.Object, model) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }
}
