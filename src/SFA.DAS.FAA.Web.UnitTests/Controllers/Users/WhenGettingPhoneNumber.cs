using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingPhoneNumber
{
    [Test]
    [MoqInlineAutoData(null, RouteNames.ConfirmAccountDetails, "What is your telephone number? – Find an apprenticeship – GOV.UK", "Create an account", "What is your telephone number?", "Continue")]
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

        var result = await controller.PhoneNumber(journeyPath) as ViewResult;
        var actualModel = result.Model as PhoneNumberViewModel;

        actualModel.Should().NotBeNull();
        actualModel!.PageTitle.Should().Be(pageTitle);
        actualModel.PageCaption.Should().Be(pageCaption);
        actualModel.PageHeading.Should().Be(pageHeading);
        actualModel.PageCtaButtonLabel.Should().Be(pageCtaButtonLabel);
        actualModel.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
    }
}
