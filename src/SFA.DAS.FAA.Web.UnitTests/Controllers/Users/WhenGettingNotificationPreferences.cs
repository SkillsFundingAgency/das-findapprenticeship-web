using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using CreateAccount.GetCandidatePreferences;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
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
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidatePreferencesQuery>(x => x.CandidateId == candidateId)
            , It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.NotificationPreferences(journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result!.Model as NotificationPreferencesViewModel;

        actualModel.Should().NotBeNull();
        actualModel!.JourneyPath.Should().Be(journeyPath);
    }
}
