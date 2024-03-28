using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.CandidatePreferences;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenPostingNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Enter_Your_Address(
        Guid candidateId,
        string email,
        NotificationPreferencesViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.NameIdentifier, candidateId.ToString())
                    }))
            }
        };

        var result = await controller.NotificationPreferences(model) as RedirectToRouteResult;

        result.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.Is<UpsertCandidatePreferencesCommand>(c =>
            c.NotificationPreferences.Count == model.NotificationPreferences.Count
            ), It.IsAny<CancellationToken>()), Times.Once);
        result.RouteName.Should().BeEquivalentTo(RouteNames.ConfirmAccountDetails);
    }
}
