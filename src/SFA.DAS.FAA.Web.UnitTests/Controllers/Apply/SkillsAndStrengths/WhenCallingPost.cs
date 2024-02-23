using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.SkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.SkillsAndStrengths;
public class WhenCallingPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        UpdateSkillsAndStrengthsApplicationCommandResult updateApplicationResult,
        GetExpectedSkillsAndStrengthsQueryResult expectedSkills,
        GetCandidateSkillsAndStrengthsQueryResult candidateSkills,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SkillsAndStrengthsController controller)
    {
        var request = new SkillsAndStrengthsViewModel(expectedSkills, candidateSkills, applicationId)
        {
            ApplicationId = Guid.NewGuid(),
            IsSectionComplete = true
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<UpdateSkillsAndStrengthsApplicationCommand>(c =>
        c.ApplicationId.Equals(request.ApplicationId)
                && c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateApplicationResult);

        mediator.Setup(x => x.Send(It.Is<CreateSkillsAndStrengthsCommand>(c =>
        c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        var actual = await controller.Post(request.ApplicationId, request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.Apply);
        }
    }
}
