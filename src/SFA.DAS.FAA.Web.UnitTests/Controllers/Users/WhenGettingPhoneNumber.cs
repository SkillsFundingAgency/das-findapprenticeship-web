using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingPhoneNumber
{
    [Test]
    [MoqInlineAutoData(null, RouteNames.SelectAddress, "What is your telephone number? – Find an apprenticeship – GOV.UK", "Create an account", "What is your telephone number?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.SelectAddress, "What is your telephone number? – Find an apprenticeship – GOV.UK", "Create an account", "What is your telephone number?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.SelectAddress, RouteNames.SelectAddress, "What is your telephone number? – Find an apprenticeship – GOV.UK", "Create an account", "What is your telephone number?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.EnterAddressManually, RouteNames.EnterAddressManually, "What is your telephone number? – Find an apprenticeship – GOV.UK", "Create an account", "What is your telephone number?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails, "What is your telephone number? – Find an apprenticeship – GOV.UK", "Create an account", "What is your telephone number?", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings, "Change your telephone number – Find an apprenticeship – GOV.UK", "", "Change your telephone number", "Save")]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string pageBackLink,
        string pageTitle,
        string pageCaption,
        string pageHeading,
        string pageCtaButtonLabel,
        string govIdentifier,
        string email,
        string phone,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        controller.WithContext(x => x
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.MobilePhone, phone)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier)
        );

        // act
        var result = await controller.PhoneNumber(journeyPath) as ViewResult;

        // assert
        result.Should().NotBeNull();
        var actualModel = result!.Model as PhoneNumberViewModel;

        actualModel.Should().NotBeNull();
        actualModel!.PageTitle.Should().Be(pageTitle);
        actualModel.PageCaption.Should().Be(pageCaption);
        actualModel.PageHeading.Should().Be(pageHeading);
        actualModel.PageCtaButtonLabel.Should().Be(pageCtaButtonLabel);
        actualModel.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
    }
}