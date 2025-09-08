using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenPostingNotificationPreferences
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings)]
    public async Task When_Model_State_Is_Valid_Should_Redirect(
        UserJourneyPath journeyPath,
        string redirectRoute,
        Guid candidateId,
        string email,
        NotificationPreferencesViewModel model,
        Mock<IValidator<NotificationPreferencesViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        model.JourneyPath = journeyPath;
        controller
            .AddControllerContext()
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email);
        validator
            .Setup(x => x.ValidateAsync(It.Is<NotificationPreferencesViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.NotificationPreferences(validator.Object, model) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.Is<UpsertCandidatePreferencesCommand>(c =>
            c.UnfinishedApplicationReminders == model.UnfinishedApplicationReminders), It.IsAny<CancellationToken>()), Times.Once);
        result!.RouteName.Should().BeEquivalentTo(redirectRoute);
        result.RouteValues["journeyPath"].Should().Be(journeyPath);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        UserJourneyPath journeyPath,
        Guid candidateId,
        string email,
        NotificationPreferencesViewModel model,
        Mock<IValidator<NotificationPreferencesViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // arrange
        model.JourneyPath = journeyPath;
        model.UnfinishedApplicationReminders = null;
        controller
            .AddControllerContext()
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email);
        validator
            .Setup(x => x.ValidateAsync(It.Is<NotificationPreferencesViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("SomeProperty", "SomeError")]));

        // act
        var result = await controller.NotificationPreferences(validator.Object, model) as ViewResult;

        // assert
        result.Should().NotBeNull();
        result!.Model.Should().Be(model);
    }
}