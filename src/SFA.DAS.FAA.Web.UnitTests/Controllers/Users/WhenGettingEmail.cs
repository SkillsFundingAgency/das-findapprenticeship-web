using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingEmail
{
    [Test]
    [MoqInlineAutoData(null, RouteNames.ConfirmAccountDetails, "Create an account")]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails, "Create an account")]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings, "")]
    public void Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string pageBackLink,
        string pageCaption,
        [Frozen] Mock<IConfiguration> config,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        config.Setup(x => x["ResourceEnvironmentName"]).Returns("Prd");
        
        var result = controller.Email(journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result!.Model as EmailViewModel;
        
        actualModel.Should().NotBeNull();
        actualModel!.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
        actualModel.PageCaption.Should().Be(pageCaption);
        actualModel.ChangeEmailLink.Should().Be("https://home.account.gov.uk/settings");
    }

    [Test]
    [MoqInlineAutoData("PrD", "https://home.account.gov.uk/settings")]
    [MoqInlineAutoData("prd", "https://home.account.gov.uk/settings")]
    [MoqInlineAutoData("asd", "https://home.integration.account.gov.uk/settings")]
    [MoqInlineAutoData("", "https://home.integration.account.gov.uk/settings")]
    public void Then_The_ChangeEmailLink_Is_Set_For_The_Environment(
        string environment,
        string expectedUrl,
        [Frozen] Mock<IConfiguration> config,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        config.Setup(x => x["ResourceEnvironmentName"]).Returns(environment);
        
        var result = controller.Email() as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result!.Model as EmailViewModel;
        actualModel!.ChangeEmailLink.Should().Be(expectedUrl);
    }
}