using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;

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
                        new Claim(ClaimTypes.Email, email),
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                    }))
            }
        };

        var result = await controller.NotificationPreferences(model) as RedirectToRouteResult;

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
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        model.JourneyPath = journeyPath;
        model.UnfinishedApplicationReminders = null;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                }))
            }
        };
        controller.ModelState.AddModelError("SomeProperty", "SomeError");

        var result = await controller.NotificationPreferences(model) as ViewResult;

        result.Should().NotBeNull();
        result!.Model.Should().Be(model);
    }
}
