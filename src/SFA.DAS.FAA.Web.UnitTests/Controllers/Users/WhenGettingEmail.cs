using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        Guid candidateId,
        string govIdentifier,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        var result = controller.Email(journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result!.Model as EmailViewModel;
        
        actualModel.Should().NotBeNull();
        actualModel!.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
    }
}