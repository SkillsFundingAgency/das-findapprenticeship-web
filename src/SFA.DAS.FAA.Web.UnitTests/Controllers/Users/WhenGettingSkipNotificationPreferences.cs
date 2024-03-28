using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingSkipNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task Then_Redirect_To_Post_NotificationPreferences_Is_Returned(
        string email,
        Guid candidateId,
        GetCandidatePreferencesQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.Email, email)
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidatePreferencesQuery>(x => x.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var result = await controller.SkipNotificationPreferences() as RedirectToRouteResult;

        result.RouteName.Should().BeEquivalentTo(RouteNames.ConfirmAccountDetails.ToString());
    }
}
