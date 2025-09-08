using CreateAccount.GetAddressesByPostcode;
using MediatR;
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
        Mock<IValidator<SelectAddressViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        model.JourneyPath = journeyPath;
        model.SelectedAddress = addressesByPostcodeQueryResult.Addresses.First().Uprn;
        controller
            .AddControllerContext()
            .WithUser(Guid.NewGuid())
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier)
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(CustomClaims.CandidateId, candidateId.ToString());
        
        mediator.Setup(x => x.Send(It.IsAny<GetAddressesByPostcodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(addressesByPostcodeQueryResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<SelectAddressViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.SelectAddress(validator.Object, model) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        result!.RouteName.Should().Be(redirectRoute);
        mediator.Verify(x => x.Send(It.IsAny<UpdateAddressCommand>(), CancellationToken.None
            ), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        GetAddressesByPostcodeQueryResult addressesByPostcodeQueryResult,
        SelectAddressViewModel model,
        Mock<IValidator<SelectAddressViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        model.SelectedAddress = "1";
        mediator.Setup(x => x.Send(It.IsAny<GetAddressesByPostcodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(addressesByPostcodeQueryResult);
        validator
            .Setup(x => x.ValidateAsync(It.Is<SelectAddressViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("SomeProperty", "SomeError")]));

        // act
        var result = await controller.SelectAddress(validator.Object, model) as ViewResult;

        // assert
        result.Should().NotBeNull();
        result.Model.As<SelectAddressViewModel>().Addresses.Should()
            .BeEquivalentTo(addressesByPostcodeQueryResult.Addresses.Select(x => (AddressViewModel)x).ToList());
    }
}